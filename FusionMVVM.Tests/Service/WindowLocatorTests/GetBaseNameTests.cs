using System;
using FusionMVVM.Service;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class GetBaseNameTests
    {
        [Theory, AutoData]
        public void GetBaseName_WhenViewModel_IsNull(WindowLocator sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.GetBaseName(null));
        }

        [Theory]
        [InlineAutoData("", "")]
        [InlineAutoData("DefaultViewModel", "Default")]
        [InlineAutoData("lowercaseviewmodel", "lowercase")]
        [InlineAutoData("DoubleViewModelViewModel", "Double")]
        public void GetBaseName_ReturnsCorrectResult(string viewModel, string expected, WindowLocator sut)
        {
            var actual = sut.GetBaseName(viewModel);
            Assert.Equal(expected, actual);
        }
    }
}
