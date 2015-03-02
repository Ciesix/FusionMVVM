using System;
using FusionMVVM.Common;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Common.LevenshteinTests
{
    public class MeasureDistanceTests
    {
        [Theory, CustomAutoData]
        public void NullSourceThrowsException(Levenshtein sut, string target)
        {
            Assert.Throws<ArgumentNullException>(() => sut.MeasureDistance(null, target));
        }

        [Theory, CustomAutoData]
        public void NullTargetThrowsException(Levenshtein sut, string source)
        {
            Assert.Throws<ArgumentNullException>(() => sut.MeasureDistance(source, null));
        }

        [Theory]
        [CustomInlineAutoData("", "", 0)]
        [CustomInlineAutoData("Sam", "Samantha", 5)]
        [CustomInlineAutoData("Kitten", "Sitting", 3)]
        [CustomInlineAutoData("FooViewModel", "Application.ViewModel.FooViewModel", 22)]
        public void MeasureDistanceReturnsCorrectResult(string source, string target, int expected, Levenshtein sut)
        {
            var actual = sut.MeasureDistance(source, target);
            Assert.Equal(expected, actual);
        }
    }
}
