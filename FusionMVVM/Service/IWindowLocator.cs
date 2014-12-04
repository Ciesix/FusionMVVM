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
    }
}
