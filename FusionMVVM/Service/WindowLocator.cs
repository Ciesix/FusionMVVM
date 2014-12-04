using System;
using System.Collections.Concurrent;
using System.Reflection;
using FusionMVVM.Common;

namespace FusionMVVM.Service
{
    public class WindowLocator : IWindowLocator
    {
        private readonly ConcurrentDictionary<Type, Type> _registeredTypes = new ConcurrentDictionary<Type, Type>();

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
    }
}
