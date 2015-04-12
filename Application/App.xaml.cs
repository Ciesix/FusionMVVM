﻿using System;
using System.Diagnostics;
using System.Reflection;
using Application.ViewModel;
using FusionMVVM;
using FusionMVVM.Common;
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
                var assembly = Assembly.GetEntryAssembly();
                IMetric metric = new Levenshtein();
                IFilter<Type> filter = new TypeEndsWithFilter();
                IStringRemove stringRemove = new ViewModelToView();

                // Add services to the Ioc container.
                Ioc.Current.RegisterAsSingleton<IWindowLocator>(new WindowLocator(assembly, metric, filter, stringRemove));

                // Register windows in the WindowLocator service.
                var windowLocator = Ioc.Current.Resolve<IWindowLocator>();
                windowLocator.RegisterAll();

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
