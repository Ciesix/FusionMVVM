using System;
using FusionMVVM.Service;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class RegisterTests
    {
        [Theory, AutoData]
        public void Register_WhenViewModelType_IsNull(WindowLocator sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Register(null));
        }

        [Theory, AutoData]
        public void RegisterManuel_WhenViewModelType_IsNull(WindowLocator sut, Type viewType)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Register(null, viewType));
        }

        [Theory, AutoData]
        public void RegisterManuel_WhenModelType_IsNull(WindowLocator sut, Type viewModelType)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Register(viewModelType, null));
        }
    }
}
