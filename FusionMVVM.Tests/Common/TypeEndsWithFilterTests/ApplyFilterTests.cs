using System;
using System.Collections.Generic;
using System.Linq;
using FusionMVVM.Common;
using FusionMVVM.Tests.TestData;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Common.TypeEndsWithFilterTests
{
    public class ApplyFilterTests
    {
        [Fact]
        public void NullTextThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var enumerable = fixture.CreateMany<Type>();
            var sut = fixture.Create<TypeEndsWithFilter>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.ApplyFilter(null, enumerable));
        }

        [Fact]
        public void NullEnumerableThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var text = fixture.Create<string>();
            var sut = fixture.Create<TypeEndsWithFilter>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.ApplyFilter(text, null));
        }

        [Theory]
        [InlineData("Bar", StringComparison.Ordinal, 0)]
        [InlineData("bar", StringComparison.OrdinalIgnoreCase, 0)]
        [InlineData("ViewModel", StringComparison.Ordinal, 1)]
        [InlineData("viewmodel", StringComparison.OrdinalIgnoreCase, 1)]
        public void ReturnsCorrectResult(string text, StringComparison comparison, int expected)
        {
            // Fixture setup.
            var fixture = new Fixture();
            var enumerable = new List<Type> { typeof(FooViewModel) };
            fixture.AddManyTo(enumerable, 4);
            var sut = new TypeEndsWithFilter();

            // Exercise system.
            var actual = sut.ApplyFilter(text, enumerable, comparison);

            // Verify outcome.
            Assert.Equal(expected, actual.Count());
        }
    }
}
