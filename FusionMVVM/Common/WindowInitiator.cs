using System;
using System.Linq;
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
            if (windowType == null) throw new ArgumentNullException("windowType");

            var constructor = windowType.GetConstructors().First();
            var activator = Activator.GetActivator(constructor);
            var window = (Window)activator();

            if (dataContext != null) window.DataContext = dataContext;
            if (ownerWindow != null) window.Owner = ownerWindow;

            return window;
        }
    }
}
