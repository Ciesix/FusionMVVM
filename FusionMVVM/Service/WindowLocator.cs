using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using FusionMVVM.Common;

namespace FusionMVVM.Service
{
    public class WindowLocator : WindowLocatorBase, IWindowLocator
    {
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
                RegisteredTypes.AddOrUpdate(viewModelType, k => viewType, (k, v) => viewType);
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
            if (OpenedWindows.TryAdd(viewModel.GetHashCode(), window))
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
            if (OpenedWindows.TryAdd(viewModel.GetHashCode(), window))
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

            var owner = UserControlOwners.FirstOrDefault(pair => pair.Value.Contains(viewModel.GetHashCode()));
            var hashCode = owner.Key != 0 ? owner.Key : viewModel.GetHashCode();

            if (OpenedWindows.TryGetValue(hashCode, out window))
            {
                // Close the window.
                window.Close();
            }
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
