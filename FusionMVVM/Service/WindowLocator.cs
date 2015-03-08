using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace FusionMVVM.Service
{
    public class WindowLocator : IWindowLocator
    {
        private readonly ConcurrentDictionary<Type, Type> _registeredTypes = new ConcurrentDictionary<Type, Type>();

        public ReadOnlyDictionary<Type, Type> RegisteredTypes
        {
            get { return new ReadOnlyDictionary<Type, Type>(_registeredTypes); }
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
            }
            else
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
    }
}
