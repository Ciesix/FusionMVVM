using System;
using System.Reflection;
using FusionMVVM.Common;
using FusionMVVM.Service;
using Ploeh.AutoFixture;
using Xunit;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class ConstructorTests
    {
        [Fact]
        public void InstantiatedWithNullAssemblyThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var metric = fixture.Create<IMetric>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(null, metric));
        }

        [Fact]
        public void InstantiatedWithNullMetricThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var assembly = fixture.Create<Assembly>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(assembly, null));
        }
    }
}
