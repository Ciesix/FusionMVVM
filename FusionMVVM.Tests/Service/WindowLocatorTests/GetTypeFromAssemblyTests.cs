using System;
using System.Reflection;
using FusionMVVM.Service;
using FusionMVVM.Tests.TestData;
using Ploeh.AutoFixture;
using Xunit;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class GetTypeFromAssemblyTests
    {
        [Fact]
        public void NullAssemblyThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();
            var typeName = fixture.Create<string>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.GetTypeFromAssembly(null, typeName));
        }

        [Fact]
        public void NullTypeNameThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();
            var assembly = fixture.Create<Assembly>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.GetTypeFromAssembly(assembly, null));
        }

        [Fact]
        public void ReturnsCorrectType()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();
            var assembly = fixture.Create<TestAssembly>();

            // Exercise system.
            var actual = sut.GetTypeFromAssembly(assembly, "FooView");

            // Verify outcome.
            Assert.Equal(typeof(FooView), actual);
        }
    }
}
