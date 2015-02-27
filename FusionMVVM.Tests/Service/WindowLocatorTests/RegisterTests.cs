using System;
using FusionMVVM.Service;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class RegisterTests
    {
        [Theory, CustomAutoData]
        public void Register_WhenViewModelType_IsNull(WindowLocator sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Register(null));
        }

        [Theory, CustomAutoData]
        public void RegisterManuel_WhenViewModelType_IsNull(WindowLocator sut, Type viewType)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Register(null, viewType));
        }

        [Theory, CustomAutoData]
        public void RegisterManuel_WhenModelType_IsNull(WindowLocator sut, Type viewModelType)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Register(viewModelType, null));
        }
    }
}
