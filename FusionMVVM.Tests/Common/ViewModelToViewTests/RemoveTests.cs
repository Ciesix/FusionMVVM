using System;
using FusionMVVM.Common;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Common.ViewModelToViewTests
{
    public class RemoveTests
    {
        [Fact]
        public void NullTextThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var sut = fixture.Create<ViewModelToView>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.Remove(null));
        }

        [Theory]
        [InlineData("", false, "")]
        [InlineData("FooViewModel", false, "FooView")]
        [InlineData("fooviewmodel", true, "fooview")]
        public void ReturnsCorrectResult(string text, bool ignoreCase, string expected)
        {
            // Fixture setup.
            var fixture = new Fixture();
            var sut = fixture.Create<ViewModelToView>();

            // Exercise system.
            var actual = sut.Remove(text, ignoreCase);

            // Verify outcome.
            Assert.Equal(expected, actual);
        }
    }
}
