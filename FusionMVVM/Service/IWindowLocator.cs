using System;

namespace FusionMVVM.Service
{
    public interface IWindowLocator
    {
        /// <summary>
        /// Registers a window using the ViewModel type as it's source.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        void Register<TViewModel>();

        /// <summary>
        /// Registers a window using the ViewModel type as it's source.
        /// </summary>
        /// <param name="viewModelType"></param>
        void Register(Type viewModelType);

        /// <summary>
        /// Registers all windows with a matching ViewModel name.
        /// </summary>
        void RegisterAll();

        /// <summary>
        /// Shows a window and sets the ViewModel as a DataContext.
        /// </summary>
        /// <param name="viewModel"></param>
        void ShowWindow(ViewModelBase viewModel);

        /// <summary>
        /// Shows a window and sets the ViewModel as a DataContext.
        /// Also set the owner of the window to the current ViewModels window.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        void ShowWindow(ViewModelBase viewModel, ViewModelBase owner);

        /// <summary>
        /// Shows a window as a dialog and sets the ViewModel as a DataContext.
        /// </summary>
        /// <param name="viewModel"></param>
        void ShowDialogWindow(ViewModelBase viewModel);

        /// <summary>
        /// Shows a window as a dialog and sets the ViewModel as a DataContext.
        /// Also set the owner of the window to the current ViewModels window.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        void ShowDialogWindow(ViewModelBase viewModel, ViewModelBase owner);

        /// <summary>
        /// Closes a window with the given ViewModel.
        /// </summary>
        /// <param name="viewModel"></param>
        void CloseWindow(ViewModelBase viewModel);
    }
}
