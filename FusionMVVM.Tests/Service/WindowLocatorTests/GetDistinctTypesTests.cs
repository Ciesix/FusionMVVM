using System;
using System.Collections.Generic;
using System.Linq;
using FusionMVVM.Common;
using FusionMVVM.Service;
using FusionMVVM.Tests.Fakes;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class GetDistinctTypesTests
    {
        [Theory, CustomAutoData]
        public void GetDistinctTypes_WhenTypes_IsNull(WindowLocator sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.GetDistinctTypes(null));
        }

        [Theory, CustomAutoData]
        public void GetDistinctTypes_WhenTypesAreEmpty_ReturnEmpty(WindowLocator sut)
        {
            var types = new List<Type>();
            var actual = sut.GetDistinctTypes(types).ToList();
            Assert.Equal(0, actual.Count);
        }

        [Theory, CustomAutoData]
        public void GetDistinctTypes_WhenResult_NotEmpty(WindowLocator sut)
        {
            var fixture = new Fixture();
            var types = fixture.CreateMany<Type>(5);

            var actual = sut.GetDistinctTypes(types).ToList();

            Assert.Equal(1, actual.Count);
        }

        [Theory]
        [PropertyData("EndsWithTypeFilterData")]
        public void GetDistinctTypes_WhenFiltered_ReturnsCorrectResult(ITypeFilter filter, int expected)
        {
            var fixture = new CustomAutoDataAttribute().Fixture;
            var sut = fixture.Create<WindowLocator>();
            var types = new List<Type> { typeof(string), typeof(FakeWindow), typeof(object), typeof(FakeCustomWindow) };

            var actual = sut.GetDistinctTypes(types, filter).ToList();

            Assert.Equal(expected, actual.Count);
        }

        public static IEnumerable<object[]> EndsWithTypeFilterData
        {
            get
            {
                return new[]
                {
                    new object[] {new EndsWithTypeFilter("Nothing"), 0},
                    new object[] {new EndsWithTypeFilter("string", false), 1},
                    new object[] {new EndsWithTypeFilter("window", false), 2},
                    new object[] {new EndsWithTypeFilter(""), 4}
                };
            }
        }

        [Theory, CustomAutoData]
        public void GetDistinctTypes_WhenFilterText_IsNull(WindowLocator sut)
        {
            var fixture = new Fixture();
            var types = fixture.CreateMany<Type>(1);

            Assert.Throws<ArgumentNullException>(() => sut.GetDistinctTypes(types, new EndsWithTypeFilter(null)));
        }
    }
}
