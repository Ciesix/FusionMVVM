using System;
using FusionMVVM.Service;
using Xunit;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class ConstructorTests
    {
        [Fact]
        public void Constructor_WhenAssembly_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(null));
        }
    }
}
