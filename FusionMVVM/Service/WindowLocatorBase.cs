using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FusionMVVM.Common;

namespace FusionMVVM.Service
{
    public abstract class WindowLocatorBase
    {
        protected readonly ConcurrentDictionary<Type, Type> RegisteredTypes = new ConcurrentDictionary<Type, Type>();
        protected readonly ConcurrentDictionary<int, Window> OpenedWindows = new ConcurrentDictionary<int, Window>();
        protected readonly ConcurrentDictionary<int, List<int>> UserControlOwners = new ConcurrentDictionary<int, List<int>>();

        /// <summary>
        /// Creates a window with or without an owner.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        protected virtual Window CreateWindow(ViewModelBase viewModel, ViewModelBase owner)
        {
            var viewModelType = viewModel.GetType();
            Window window = null;
            Type viewType;

            if (RegisteredTypes.TryGetValue(viewModelType, out viewType))
            {
                // Create the window.
                var constructor = viewType.GetConstructors().First();
                var activator = Common.Activator.GetActivator(constructor);

                window = (Window)activator();
                window.DataContext = viewModel;
                window.Closed += Window_OnClosed;

                Window ownerWindow;
                if (owner != null && OpenedWindows.TryGetValue(owner.GetHashCode(), out ownerWindow))
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
        protected virtual void AutoloadUserControls(Window window)
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
                    var owner = UserControlOwners.GetOrAdd(viewModel.GetHashCode(), k => new List<int>());

                    // Find the property with the same name as the ContentControl.
                    var property = viewModel.GetType().GetProperties().FirstOrDefault(propertyInfo => propertyInfo.Name == propertyName);

                    if (property != null && RegisteredTypes.TryGetValue(property.PropertyType, out viewType))
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
        protected virtual void Window_OnClosed(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (window == null) return;

            window.Closed -= Window_OnClosed;

            var viewModel = window.DataContext;
            if (viewModel == null) return;

            Window closedWindow;
            OpenedWindows.TryRemove(viewModel.GetHashCode(), out closedWindow);
        }

        /// <summary>
        /// Returns a collection of ViewModel types from given assembly types.
        /// </summary>
        /// <param name="assemblyTypes"></param>
        /// <returns></returns>
        protected virtual IEnumerable<Type> GetViewModelTypes(IEnumerable<Type> assemblyTypes)
        {
            if (assemblyTypes == null) throw new ArgumentNullException("assemblyTypes");

            var result = from type in assemblyTypes
                         where type.FullName != null && (type.IsClass && type.FullName.EndsWith("ViewModel", StringComparison.OrdinalIgnoreCase))
                         select type;

            return result;
        }
    }
}
