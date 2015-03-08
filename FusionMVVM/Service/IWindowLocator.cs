using System;

namespace FusionMVVM.Service
{
    public interface IWindowLocator
    {
        /// <summary>
        /// Registers a ViewModel and View with the same base name.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        void Register<TViewModel>();

        /// <summary>
        /// Registers a ViewModel and View as a pair.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TView"></typeparam>
        void Register<TViewModel, TView>();

        /// <summary>
        /// Registers a ViewModel and View as a pair. If a ViewType is not
        /// specified, a View with the same base name will be registered to
        /// the provided ViewModel.
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="viewType"></param>
        void Register(Type viewModelType, Type viewType = null);

        void RegisterAll(bool includeReferencedAssemblies = false);

        void ShowWindow(ViewModelBase viewModel, ViewModelBase owner = null);

        void ShowDialogWindow(ViewModelBase viewModel, ViewModelBase owner = null);

        void CloseWindow(ViewModelBase viewModel);
    }
}
