using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Windows;
using FusionMVVM.Common;
using Activator = FusionMVVM.Common.Activator;

namespace FusionMVVM.Service
{
    public class WindowLocator : IWindowLocator
    {
        private readonly ConcurrentDictionary<Type, Type> _registeredTypes = new ConcurrentDictionary<Type, Type>();
        private readonly ConcurrentDictionary<int, Window> _openedWindows = new ConcurrentDictionary<int, Window>();

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

            if (viewType != null)
            {
                _registeredTypes.AddOrUpdate(viewModelType, k => viewType, (k, v) => viewType);
            }
        }

        /// <summary>
        /// Shows a window and sets the ViewModel as a DataContext.
        /// </summary>
        /// <param name="viewModel"></param>
        public void ShowWindow(BaseViewModel viewModel)
        {
            ShowWindow(viewModel, null);
        }

        /// <summary>
        /// Shows a window and sets the ViewModel as a DataContext.
        /// Also set the owner of the window to the current ViewModels window.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        public void ShowWindow(BaseViewModel viewModel, BaseViewModel owner)
        {
            var window = CreateWindow(viewModel, owner);
            if (_openedWindows.TryAdd(viewModel.GetHashCode(), window))
            {
                window.Show();
            }
        }

        /// <summary>
        /// Gets the base name of the ViewModel name.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private string GetBaseName(string viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel)) throw new ArgumentNullException("viewModel");

            var index = viewModel.LastIndexOf("viewmodel", StringComparison.OrdinalIgnoreCase);
            if (index != -1)
            {
                var name = viewModel.Substring(0, index);
                return name;
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates a window with or without an owner.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        private Window CreateWindow(BaseViewModel viewModel, BaseViewModel owner)
        {
            var viewModelType = viewModel.GetType();
            Window window = null;
            Type viewType;

            if (_registeredTypes.TryGetValue(viewModelType, out viewType))
            {
                // Create the window.
                var constructor = viewType.GetConstructors().First();
                var activator = Activator.GetActivator(constructor);

                window = (Window)activator();
                window.DataContext = viewModel;
                window.Closed += Window_OnClosed;

                Window ownerWindow;
                if (owner != null && _openedWindows.TryGetValue(owner.GetHashCode(), out ownerWindow))
                {
                    // Set the window owner to the ownerWindow.
                    window.Owner = ownerWindow;
                }
            }

            return window;
        }

        /// <summary>
        /// Cleanup after the window has been closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_OnClosed(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (window == null) return;

            window.Closed -= Window_OnClosed;

            var viewModel = window.DataContext;
            if (viewModel == null) return;

            Window closedWindow;
            _openedWindows.TryRemove(viewModel.GetHashCode(), out closedWindow);
        }
    }
}
