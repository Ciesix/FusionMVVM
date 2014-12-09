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
                // Add services to the Ioc container.
                Ioc.Current.RegisterAsSingleton<IWindowLocator>(new WindowLocator());

                // Register windows in the WindowLocator service.
                var windowLocator = Ioc.Current.Resolve<IWindowLocator>();
                windowLocator.Register<MainViewModel>();

                // Shows the main window.
                windowLocator.ShowWindow(new MainViewModel());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (Debugger.IsAttached) Debugger.Break();
            }
        }
    }
}
