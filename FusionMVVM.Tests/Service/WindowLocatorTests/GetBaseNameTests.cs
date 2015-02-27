using System;
using FusionMVVM.Service;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class GetBaseNameTests
    {
        [Theory, CustomAutoData]
        public void GetBaseName_WhenViewModel_IsNull(WindowLocator sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.GetBaseName(null));
        }

        [Theory]
        [CustomInlineAutoData("", "")]
        [CustomInlineAutoData("DefaultViewModel", "Default")]
        [CustomInlineAutoData("lowercaseviewmodel", "lowercase")]
        [CustomInlineAutoData("DoubleViewModelViewModel", "Double")]
        public void GetBaseName_ReturnsCorrectResult(string viewModel, string expected, WindowLocator sut)
        {
            var actual = sut.GetBaseName(viewModel);
            Assert.Equal(expected, actual);
        }
    }
}
