using System;
using System.Reflection;
using FusionMVVM.Common;
using FusionMVVM.Tests.TestData;
using Ploeh.AutoFixture;
using Xunit;

namespace FusionMVVM.Tests.Common.AssemblyExtensionsTests
{
    public class GetTypeTests
    {
        [Fact]
        public void NullMetricThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var sut = fixture.Create<Assembly>();
            var typeName = fixture.Create<string>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.GetType(null, typeName));
        }

        [Fact]
        public void NullTypeNameThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<Assembly>();
            var metric = fixture.Create<IMetric>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.GetType(metric, null));
        }

        [Fact]
        public void ReturnsCorrectResult()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<TestAssembly>();
            var metric = fixture.Create<IMetric>();

            // Exercise system.
            var actual = sut.GetType(metric, "FooView");

            // Verify outcome.
            Assert.Equal(typeof(FooView), actual);
        }
    }
}
