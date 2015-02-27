using System;
using FusionMVVM.Common;
using FusionMVVM.Service;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class ConstructorTests
    {
        [Theory, CustomAutoData]
        public void Constructor_WhenAssembly_IsNull(IWindowInitiator windowInitiator)
        {
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(null, windowInitiator));
        }
    }
}
