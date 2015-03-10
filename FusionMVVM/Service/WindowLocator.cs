using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FusionMVVM.Common;

namespace FusionMVVM.Service
{
    public class WindowLocator : IWindowLocator
    {
        private readonly ConcurrentDictionary<Type, Type> _registeredTypes = new ConcurrentDictionary<Type, Type>();

        private readonly IMetric _metric;

        public ReadOnlyDictionary<Type, Type> RegisteredTypes
        {
            get { return new ReadOnlyDictionary<Type, Type>(_registeredTypes); }
        }

        /// <summary>
        /// Initializes a new instance of the WindowLocator class.
        /// </summary>
        /// <param name="metric"></param>
        public WindowLocator(IMetric metric)
        {
            if (metric == null) throw new ArgumentNullException("metric");

            _metric = metric;
        }

        /// <summary>
        /// Registers a ViewModel and View with the same base name.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public void Register<TViewModel>()
        {
            Register(typeof(TViewModel));
        }

        /// <summary>
        /// Registers a ViewModel and View as a pair.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TView"></typeparam>
        public void Register<TViewModel, TView>()
        {
            Register(typeof(TViewModel), typeof(TView));
        }

        /// <summary>
        /// Registers a ViewModel and View as a pair. If a ViewType is not
        /// specified, a View with the same base name will be registered to
        /// the provided ViewModel.
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="viewType"></param>
        public void Register(Type viewModelType, Type viewType = null)
        {
            if (viewModelType == null) throw new ArgumentNullException("viewModelType");

            if (viewType == null)
            {
                var assembly = viewModelType.Assembly;
                var viewName = Regex.Replace(viewModelType.Name, "Model", string.Empty, RegexOptions.IgnoreCase);
                viewType = GetTypeFromAssembly(assembly, viewName);
            }

            if (viewType != null)
            {
                _registeredTypes.AddOrUpdate(viewModelType, k => viewType, (k, v) => viewType);
            }
        }

        public void RegisterAll(bool includeReferencedAssemblies = false)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(ViewModelBase viewModel, ViewModelBase owner = null)
        {
            throw new NotImplementedException();
        }

        public void ShowDialogWindow(ViewModelBase viewModel, ViewModelBase owner = null)
        {
            throw new NotImplementedException();
        }

        public void CloseWindow(ViewModelBase viewModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to locate a type by name in the provided assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type GetTypeFromAssembly(Assembly assembly, string typeName)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (typeName == null) throw new ArgumentNullException("typeName");

            var distinctTypes = assembly.GetTypes().Distinct();
            var types = distinctTypes.Where(type => type.Name.Contains(typeName));
            var shortestDistance = int.MaxValue;
            Type bestMatchingType = null;

            foreach (var type in types)
            {
                var distance = _metric.MeasureDistance(type.Name, typeName);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestMatchingType = type;
                }
            }

            return bestMatchingType;
        }
    }
}
