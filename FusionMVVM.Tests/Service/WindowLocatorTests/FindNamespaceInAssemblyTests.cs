using System;
using System.Reflection;
using FusionMVVM.Service;
using FusionMVVM.Tests.Fakes;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class FindNamespaceInAssemblyTests
    {
        [Theory, AutoData]
        public void FindNamespaceInAssembly_WhenAssembly_IsNull(WindowLocator sut, string name)
        {
            Assert.Throws<ArgumentNullException>(() => sut.FindNamespaceInAssembly(null, name));
        }

        [Theory, AutoData]
        public void FindNamespaceInAssembly_WhenName_IsNull(WindowLocator sut, Assembly assembly)
        {
            Assert.Throws<ArgumentNullException>(() => sut.FindNamespaceInAssembly(assembly, null));
        }

        [Theory]
        [InlineAutoData("", "")]
        [InlineAutoData(" ", "")]
        [InlineAutoData("String", "System.String")]
        public void FindNamespaceInAssembly_ReturnsCorrectResult(string target, string expected, WindowLocator sut, TestAssembly assembly)
        {
            var actual = sut.FindNamespaceInAssembly(assembly, target);
            Assert.Equal(expected, actual);
        }
    }
}
