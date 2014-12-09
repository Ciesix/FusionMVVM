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
        /// Shows a window and sets the ViewModel as a DataContext.
        /// </summary>
        /// <param name="viewModel"></param>
        void ShowWindow(BaseViewModel viewModel);

        /// <summary>
        /// Shows a window and sets the ViewModel as a DataContext.
        /// Also set the owner of the window to the current ViewModels window.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        void ShowWindow(BaseViewModel viewModel, BaseViewModel owner);

        /// <summary>
        /// Closes a window with the given ViewModel.
        /// </summary>
        /// <param name="viewModel"></param>
        void CloseWindow(BaseViewModel viewModel);
    }
}
