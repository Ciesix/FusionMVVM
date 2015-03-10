using System;
using FusionMVVM.Service;
using Xunit;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class ConstructorTests
    {
        [Fact]
        public void InstantiatedWithNullMetricThrowsException()
        {
            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(null));
        }
    }
}
