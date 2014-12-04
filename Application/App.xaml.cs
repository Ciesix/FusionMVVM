using System;
using System.Diagnostics;
using Application.ViewModel;
using FusionMVVM;
using FusionMVVM.Service;

namespace Application
{
    public partial class App
    {
        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        public App()
        {
            try
            {
                Ioc.Current.RegisterAsSingleton<IWindowLocator>(new WindowLocator());

                var windowLocator = Ioc.Current.Resolve<IWindowLocator>();
                windowLocator.Register<MainViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (Debugger.IsAttached) Debugger.Break();
            }
        }
    }
}
