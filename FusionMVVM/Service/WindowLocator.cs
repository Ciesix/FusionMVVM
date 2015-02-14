using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using FusionMVVM.Common;

namespace FusionMVVM.Service
{
    public class WindowLocator : IWindowLocator
    {
        private readonly ConcurrentDictionary<Type, Type> _registeredTypes = new ConcurrentDictionary<Type, Type>();
        private readonly ConcurrentDictionary<int, Window> _openedWindows = new ConcurrentDictionary<int, Window>();
        private readonly ConcurrentDictionary<int, List<int>> _userControlOwners = new ConcurrentDictionary<int, List<int>>();

        private readonly Assembly _assembly;

        /// <summary>
        /// Initializes a new instance of the WindowLocator class.
        /// </summary>
        /// <param name="assembly"></param>
        public WindowLocator(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            _assembly = assembly;
        }

        /// <summary>
        /// Registers a window using the ViewModel type as it's source.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public void Register<TViewModel>()
        {
            Register(typeof(TViewModel));
        }

        /// <summary>
        /// Registers a window using the ViewModel type as it's source.
        /// </summary>
        /// <param name="viewModelType"></param>
        public void Register(Type viewModelType)
        {
            if (viewModelType == null) throw new ArgumentNullException("viewModelType");

            var name = GetBaseName(viewModelType.Name);
            var assembly = viewModelType.Assembly;
            var viewName = FindNamespaceInAssembly(assembly, name + "View");
            var viewType = ConvertNameToType(viewName, assembly);
            var isTypeValid = VerifyValidBaseType(viewType, new List<Type> { typeof(Window), typeof(UserControl) });

            if (viewType != null && isTypeValid)
            {
                _registeredTypes.AddOrUpdate(viewModelType, k => viewType, (k, v) => viewType);
            }
        }

        /// <summary>
        /// Registers all windows with a matching ViewModel name in the entry types.
        /// </summary>
        public void RegisterAll()
        {
            RegisterAll(false);
        }

        /// <summary>
        /// Registers all windows with a matching ViewModel name in the entry types.
        /// If includeReferencedAssemblies is true, all referenced assemblies are also searched.
        /// </summary>
        /// <param name="includeReferencedAssemblies"></param>
        public void RegisterAll(bool includeReferencedAssemblies)
        {
            var assemblies = new List<Assembly> { _assembly };

            if (includeReferencedAssemblies)
            {
                var referencedAssemblies = _assembly.GetReferencedAssemblies();
                assemblies.AddRange(referencedAssemblies.Select(Assembly.Load));
            }

            foreach (var assembly in assemblies)
            {
                foreach (var viewModelType in GetViewModelTypes(assembly.GetTypes()))
                {
                    Register(viewModelType);
                }
            }
        }

        /// <summary>
        /// Shows a window and sets the ViewModel as a DataContext.
        /// </summary>
        /// <param name="viewModel"></param>
        public void ShowWindow(ViewModelBase viewModel)
        {
            ShowWindow(viewModel, null);
        }

        /// <summary>
        /// Shows a window and sets the ViewModel as a DataContext.
        /// Also set the owner of the window to the current ViewModels window.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        public void ShowWindow(ViewModelBase viewModel, ViewModelBase owner)
        {
            var window = CreateWindow(viewModel, owner);
            if (_openedWindows.TryAdd(viewModel.GetHashCode(), window))
            {
                window.Show();
            }
        }

        /// <summary>
        /// Shows a window as a dialog and sets the ViewModel as a DataContext.
        /// </summary>
        /// <param name="viewModel"></param>
        public void ShowDialogWindow(ViewModelBase viewModel)
        {
            ShowDialogWindow(viewModel, null);
        }

        /// <summary>
        /// Shows a window as a dialog and sets the ViewModel as a DataContext.
        /// Also set the owner of the window to the current ViewModels window.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        public void ShowDialogWindow(ViewModelBase viewModel, ViewModelBase owner)
        {
            var window = CreateWindow(viewModel, owner);
            if (_openedWindows.TryAdd(viewModel.GetHashCode(), window))
            {
                window.ShowDialog();
            }
        }

        /// <summary>
        /// Closes a window with the given ViewModel.
        /// </summary>
        /// <param name="viewModel"></param>
        public void CloseWindow(ViewModelBase viewModel)
        {
            Window window;

            var owner = _userControlOwners.FirstOrDefault(pair => pair.Value.Contains(viewModel.GetHashCode()));
            var hashCode = owner.Key != 0 ? owner.Key : viewModel.GetHashCode();

            if (_openedWindows.TryGetValue(hashCode, out window))
            {
                // Close the window.
                window.Close();
            }
        }

        /// <summary>
        /// Creates a window with or without an owner.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public Window CreateWindow(ViewModelBase viewModel, ViewModelBase owner)
        {
            var viewModelType = viewModel.GetType();
            Window window = null;
            Type viewType;

            if (_registeredTypes.TryGetValue(viewModelType, out viewType))
            {
                // Create the window.
                var constructor = viewType.GetConstructors().First();
                var activator = Common.Activator.GetActivator(constructor);

                window = (Window)activator();
                window.DataContext = viewModel;
                window.Closed += Window_OnClosed;

                Window ownerWindow;
                if (owner != null && _openedWindows.TryGetValue(owner.GetHashCode(), out ownerWindow))
                {
                    // Set the window owner to the ownerWindow.
                    window.Owner = ownerWindow;
                }

                AutoloadUserControls(window);
            }

            return window;
        }

        /// <summary>
        /// Automatically load UserControls with matching names.
        /// </summary>
        /// <param name="window"></param>
        public void AutoloadUserControls(Window window)
        {
            if (window == null) throw new ArgumentNullException("window");

            // Finds all ContentControls in the window, that has a name.
            var contentControls = window.FindLogicalChildren<ContentControl>().Where(control => string.IsNullOrWhiteSpace(control.Name) == false);

            foreach (var contentControl in contentControls)
            {
                var propertyName = contentControl.Name;
                var viewModel = contentControl.DataContext as ViewModelBase;

                if (viewModel != null)
                {
                    Type viewType;

                    // Gets a collection of the owners UserControl's.
                    var owner = _userControlOwners.GetOrAdd(viewModel.GetHashCode(), k => new List<int>());

                    // Find the property with the same name as the ContentControl.
                    var property = viewModel.GetType().GetProperties().FirstOrDefault(propertyInfo => propertyInfo.Name == propertyName);

                    if (property != null && _registeredTypes.TryGetValue(property.PropertyType, out viewType))
                    {
                        // Create the UserControl object.
                        var constructor = viewType.GetConstructors().First();
                        var activator = Common.Activator.GetActivator(constructor);
                        var userControl = (UserControl)activator();

                        // Attach the child ViewModel and View.
                        var dataContext = property.GetValue(viewModel);
                        contentControl.DataContext = dataContext;
                        contentControl.Content = userControl;

                        // Adds the UserControl's ViewModel to it's owner.
                        if (dataContext != null) owner.Add(dataContext.GetHashCode());
                    }
                }
            }
        }

        /// <summary>
        /// Cleanup after the window has been closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_OnClosed(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (window == null) return;

            window.Closed -= Window_OnClosed;

            var viewModel = window.DataContext;
            if (viewModel == null) return;

            Window closedWindow;
            _openedWindows.TryRemove(viewModel.GetHashCode(), out closedWindow);
        }

        /// <summary>
        /// Returns a collection of ViewModel types from given assembly types.
        /// </summary>
        /// <param name="assemblyTypes"></param>
        /// <returns></returns>
        public IEnumerable<Type> GetViewModelTypes(IEnumerable<Type> assemblyTypes)
        {
            if (assemblyTypes == null) throw new ArgumentNullException("assemblyTypes");

            var result = from type in assemblyTypes
                         where type.FullName != null && (type.IsClass && type.FullName.EndsWith("ViewModel", StringComparison.OrdinalIgnoreCase))
                         select type;

            return result;
        }

        /// <summary>
        /// Gets the base name of the ViewModel. Will return 'Foo' if the
        /// ViewModel value is 'FooViewModel'.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public string GetBaseName(string viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            var name = string.Empty;
            var index = viewModel.IndexOf("ViewModel", StringComparison.OrdinalIgnoreCase);

            if (index != -1)
            {
                name = viewModel.Substring(0, index);
            }

            return name;
        }

        /// <summary>
        /// Gets a distinct list of types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public IEnumerable<string> GetDistinctTypes(IEnumerable<Type> types)
        {
            if (types == null) throw new ArgumentNullException("types");
            return types.Select(t => t.FullName).Distinct();
        }

        /// <summary>
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public string FindNamespaceInAssembly(Assembly assembly, string target)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (target == null) throw new ArgumentNullException("target");

            if (target.Trim() == string.Empty) return string.Empty;

            var namespaces = GetDistinctTypes(assembly.GetTypes()).ToList();
            var foundNamespaces = namespaces.FindAll(ns => ns.Contains(target));
            var shortestDistance = int.MaxValue;
            var result = string.Empty;

            foreach (var item in foundNamespaces)
            {
                var distance = Levenshtein.MeasureDistance(item, target);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    result = item;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a type name to a type within the assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public Type ConvertNameToType(string typeName, Assembly assembly)
        {
            if (typeName == null) throw new ArgumentNullException("typeName");
            if (assembly == null) throw new ArgumentNullException("assembly");

            return Type.GetType(typeName + ", " + assembly.FullName);
        }

        /// <summary>
        /// Verifies whether the base type is a part of the valid types.
        /// </summary>
        /// <param name="currentType"></param>
        /// <param name="validTypes"></param>
        /// <returns></returns>
        public bool VerifyValidBaseType(Type currentType, IEnumerable<Type> validTypes)
        {
            if (currentType == null) throw new ArgumentNullException("currentType");
            if (validTypes == null) throw new ArgumentNullException("validTypes");

            var baseType = currentType.BaseType;
            var copyValidTypes = validTypes as IList<Type> ?? validTypes.ToList();
            var isValid = copyValidTypes.Any(t => t == baseType);

            if (baseType == null || baseType == typeof(UIElement)) return false;
            return isValid || VerifyValidBaseType(baseType, copyValidTypes);
        }
    }
}
