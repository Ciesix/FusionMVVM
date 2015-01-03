using System;
using FusionMVVM.Tests.Fakes;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests
{
    public class ContainerBaseTests
    {
        [Fact]
        public void CleanupServices_Null_ThrowException()
        {
            var sut = new FakeContainerBase();
            Assert.Throws<ArgumentNullException>(() => sut.CleanupServices(null, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("MSSQL")]
        public void CleanupServices_ServiceRemovedCorrect(string name)
        {
            var sut = new FakeContainerBase();
            sut.AddStoredServices(typeof(IFakeDatabaseService), new FakeDatabaseService(), name);
            Assert.Equal(1, sut.CountStoredServices);

            sut.CleanupServices(typeof(IFakeDatabaseService), name);
            Assert.Equal(0, sut.CountStoredServices);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("  ", false)]
        [InlineData("MSSQL", true)]
        [InlineData("123456", true)]
        [InlineData("-.", false)]
        public void IsNameNullOrValid_ReturnsCorrectResult(string name, bool expected)
        {
            var sut = new FakeContainerBase();
            var actual = sut.IsNameNullOrValid(name);
            Assert.Equal(expected, actual);
        }
    }
}
