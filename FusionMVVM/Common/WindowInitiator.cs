using System;
using System.Windows;

namespace FusionMVVM.Common
{
    public class WindowInitiator : IWindowInitiator
    {
        /// <summary>
        /// Initializes and returns a new Window, with the available
        /// DataContext and owner.
        /// </summary>
        /// <param name="windowType"></param>
        /// <param name="dataContext"></param>
        /// <param name="ownerWindow"></param>
        /// <returns></returns>
        public Window Initialize(Type windowType, object dataContext, Window ownerWindow)
        {
            throw new NotImplementedException();
        }
    }
}
