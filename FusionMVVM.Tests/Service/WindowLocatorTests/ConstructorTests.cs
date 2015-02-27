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
        public void InitializedWithNullAssemblyThrowsException(IWindowInitiator windowInitiator)
        {
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(null, windowInitiator));
        }

        [Theory, CustomAutoData]
        public void InitializedWithNullWindowInitiatorThrowsException(Assembly assembly)
        {
            Assert.Throws<ArgumentNullException>(() => new WindowLocator(assembly, null));
        }

        [Theory, CustomAutoData]
        public void InitializedReturnsCorrectResult(Assembly assembly, IWindowInitiator windowInitiator)
        {
            var sut = new WindowLocator(assembly, windowInitiator);
            Assert.NotNull(sut);
        }
    }
}
