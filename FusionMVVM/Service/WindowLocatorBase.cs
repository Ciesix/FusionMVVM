﻿using System;
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

        /// <summary>
        /// Gets the base name of the ViewModel name.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        protected virtual string GetBaseName(string viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");

            // Trim spaces.
            viewModel = viewModel.Trim();

            // Find the index of 'ViewModel'.
            var index = viewModel.IndexOf("ViewModel", StringComparison.Ordinal);
            if (index != -1)
            {
                // Trim 'ViewModel' from the name.
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

                    // Find the property with the same name as the ContentControl.
                    var property = viewModel.GetType().GetProperties().FirstOrDefault(propertyInfo => propertyInfo.Name == propertyName);

                    if (property != null && RegisteredTypes.TryGetValue(property.PropertyType, out viewType))
                    {
                        // Create the UserControl object.
                        var constructor = viewType.GetConstructors().First();
                        var activator = Common.Activator.GetActivator(constructor);
                        var userControl = (UserControl)activator();

                        // Attach the child ViewModel and View.
                        contentControl.DataContext = property.GetValue(viewModel);
                        contentControl.Content = userControl;
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
