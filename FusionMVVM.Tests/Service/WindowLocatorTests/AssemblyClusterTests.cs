using System;
using System.Linq;
using System.Reflection;
using FusionMVVM.Service;
using FusionMVVM.Tests.Fakes;
using Ploeh.AutoFixture;
using Xunit;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class AssemblyClusterTests
    {
        [Fact]
        public void NullAssemblyThrowsException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.AssemblyCluster(null));
        }

        [Fact]
        public void ReturnsSameAssembly()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();
            var assembly = fixture.Create<Assembly>();

            // Exercise system.
            var actual = sut.AssemblyCluster(assembly).Single();

            // Verify outcome.
            Assert.Same(assembly, actual);
        }

        [Fact]
        public void IncludeReferencedAssemblies()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();
            var assembly = fixture.Create<TestAssembly>();

            // Exercise system.
            var actual = sut.AssemblyCluster(assembly, true);

            // Verify outcome.
            Assert.Equal(5, actual.Count);
        }
    }
}
