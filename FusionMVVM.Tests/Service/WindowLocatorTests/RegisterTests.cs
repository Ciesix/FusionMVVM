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
    }
}
