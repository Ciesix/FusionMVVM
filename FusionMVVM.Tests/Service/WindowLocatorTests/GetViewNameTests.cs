using System;
using FusionMVVM.Service;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class GetViewNameTests
    {
        [Fact]
        public void NullViewModelNameThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.GetViewName(null));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("FooViewModel", "FooView")]
        [InlineData("fooviewmodel", "fooview")]
        public void ReturnsCorrectResult(string viewModelName, string expected)
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();

            // Exercise system.
            var actual = sut.GetViewName(viewModelName);

            // Verify outcome.
            Assert.Equal(expected, actual);
        }
    }
}
