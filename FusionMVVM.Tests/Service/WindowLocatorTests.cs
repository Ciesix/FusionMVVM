using System;
using FusionMVVM.Service;
using Xunit;

namespace FusionMVVM.Tests.Service
{
    public class WindowLocatorTests
    {
        [Fact]
        public void Register_ViewModelTypeParameterIsNull_ThrowException()
        {
            var sut = new WindowLocator();
            var exception = Assert.Throws<ArgumentNullException>(() => sut.Register(null));
            Assert.Equal("viewModelType", exception.ParamName);
        }
    }
}
