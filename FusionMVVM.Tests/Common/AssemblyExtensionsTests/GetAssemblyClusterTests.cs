using System.Linq;
using System.Reflection;
using FusionMVVM.Common;
using FusionMVVM.Tests.TestData;
using Ploeh.AutoFixture;
using Xunit;

namespace FusionMVVM.Tests.Common.AssemblyExtensionsTests
{
    public class GetAssemblyClusterTests
    {
        [Fact]
        public void ReturnsSameAssembly()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<Assembly>();

            // Exercise system.
            var actual = sut.GetAssemblyCluster().Single();

            // Verify outcome.
            Assert.Same(sut, actual);
        }

        [Fact]
        public void IncludeReferencedAssemblies()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<TestAssembly>();

            // Exercise system.
            var actual = sut.GetAssemblyCluster(true);

            // Verify outcome.
            Assert.Equal(5, actual.Count);
        }
    }
}
