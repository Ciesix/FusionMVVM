using System;
using System.Reflection;
using FusionMVVM.Common;
using FusionMVVM.Service;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class ConstructorTests
    {
        [Theory, CustomAutoData]
        public void InitializedWithNullAssemblyThrowsException(IWindowInitiator windowInitiator, IMetric metric)
        {
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(null, windowInitiator, metric));
        }

        [Theory, CustomAutoData]
        public void InitializedWithNullWindowInitiatorThrowsException(Assembly assembly, IMetric metric)
        {
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(assembly, null, metric));
        }

        [Theory, CustomAutoData]
        public void InitializedWithNullMetricThrowsException(Assembly assembly, IMetric metric, IWindowInitiator windowInitiator)
        {
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(assembly, windowInitiator, null));
        }

        [Theory, CustomAutoData]
        public void InitializedReturnsCorrectResult(Assembly assembly, IWindowInitiator windowInitiator, IMetric metric)
        {
            var sut = new WindowLocator(assembly, windowInitiator, metric);
            Assert.NotNull(sut);
        }
    }
}
