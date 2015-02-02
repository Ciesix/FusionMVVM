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
            var assembly = Assembly.GetEntryAssembly();

            // Find View in the entry assembly.
            var viewName = NamespaceHelper.FindNamespace(assembly, name + "View");

            // Convert the View name to a type.
            var viewType = Type.GetType(viewName + ", " + assembly.FullName);

            if (viewType != null && (viewType.BaseType == typeof(Window) || viewType.BaseType == typeof(UserControl)))
            {
                RegisteredTypes.AddOrUpdate(viewModelType, k => viewType, (k, v) => viewType);
            }
        }

        /// <summary>
        /// Registers all windows with a matching ViewModel name in the entry assembly.
        /// </summary>
        public void RegisterAll()
        {
            RegisterAll(false);
        }

        /// <summary>
        /// Registers all windows with a matching ViewModel name in the entry assembly.
        /// If includeReferencedAssemblies is true, all referenced assemblies are also searched.
        /// </summary>
        /// <param name="includeReferencedAssemblies"></param>
        public void RegisterAll(bool includeReferencedAssemblies)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var assemblies = new List<Assembly> { entryAssembly };

            if (includeReferencedAssemblies)
            {
                var referencedAssemblies = entryAssembly.GetReferencedAssemblies();
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
            if (OpenedWindows.TryGetValue(viewModel.GetHashCode(), out window))
            {
                // Close the window.
                window.Close();
            }
        }
    }
}
