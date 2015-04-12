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
        public void NullAssemblyThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var metric = fixture.Create<IMetric>();
            var filter = fixture.Create<IFilter<Type>>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(null, metric, filter));
        }

        [Fact]
        public void NullMetricThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var assembly = fixture.Create<Assembly>();
            var filter = fixture.Create<IFilter<Type>>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(assembly, null, filter));
        }

        [Fact]
        public void NullFilterThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var assembly = fixture.Create<Assembly>();
            var metric = fixture.Create<IMetric>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(assembly, metric, null));
        }
    }
}
