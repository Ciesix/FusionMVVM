using System;
using System.Collections.Generic;
using System.Linq;
using FusionMVVM.Service;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class GetDistinctTypesTests
    {
        [Theory, AutoData]
        public void GetDistinctTypes_WhenTypes_IsNull(WindowLocator sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.GetDistinctTypes(null));
        }

        [Theory, AutoData]
        public void GetDistinctTypes_WhenTypesAreEmpty_ReturnEmpty(WindowLocator sut)
        {
            var types = new List<Type>();
            var actual = sut.GetDistinctTypes(types).ToList();
            Assert.Equal(0, actual.Count);
        }

        [Theory, AutoData]
        public void GetDistinctTypes_WhenResult_NotEmpty(WindowLocator sut)
        {
            var fixture = new Fixture();
            var types = fixture.CreateMany<Type>(5);

            var actual = sut.GetDistinctTypes(types).ToList();

            Assert.Equal(1, actual.Count);
        }
    }
}
