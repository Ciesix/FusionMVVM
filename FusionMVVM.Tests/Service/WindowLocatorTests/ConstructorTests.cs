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
            var stringRemove = fixture.Create<IStringRemove>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(null, metric, filter, stringRemove));
        }

        [Fact]
        public void NullMetricThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var assembly = fixture.Create<Assembly>();
            var filter = fixture.Create<IFilter<Type>>();
            var stringRemove = fixture.Create<IStringRemove>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(assembly, null, filter, stringRemove));
        }

        [Fact]
        public void NullFilterThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var assembly = fixture.Create<Assembly>();
            var metric = fixture.Create<IMetric>();
            var stringRemove = fixture.Create<IStringRemove>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(assembly, metric, null, stringRemove));
        }

        [Fact]
        public void NullStringRemoveThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var assembly = fixture.Create<Assembly>();
            var metric = fixture.Create<IMetric>();
            var filter = fixture.Create<IFilter<Type>>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(assembly, metric, filter, null));
        }
    }
}
