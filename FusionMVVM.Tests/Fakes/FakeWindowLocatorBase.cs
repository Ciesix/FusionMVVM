using System;
using System.Collections.Generic;
using FusionMVVM.Service;

namespace FusionMVVM.Tests.Fakes
{
    public class FakeWindowLocatorBase : WindowLocatorBase
    {
        public new string GetBaseName(string viewModel)
        {
            return base.GetBaseName(viewModel);
        }

        public new IEnumerable<Type> GetViewModelTypes(IEnumerable<Type> assemblyTypes)
        {
            return base.GetViewModelTypes(assemblyTypes);
        }
    }
}
