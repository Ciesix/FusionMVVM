using System;
using System.Reflection;
using FusionMVVM.Service;
using FusionMVVM.Tests.Fakes;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class FindNamespaceInAssemblyTests
    {
        [Theory, CustomAutoData]
        public void FindNamespaceInAssembly_WhenAssembly_IsNull(WindowLocator sut, string name)
        {
            Assert.Throws<ArgumentNullException>(() => sut.FindNamespaceInAssembly(null, name));
        }

        [Theory, CustomAutoData]
        public void FindNamespaceInAssembly_WhenName_IsNull(WindowLocator sut, Assembly assembly)
        {
            Assert.Throws<ArgumentNullException>(() => sut.FindNamespaceInAssembly(assembly, null));
        }

        [Theory]
        [CustomInlineAutoData("", "")]
        [CustomInlineAutoData(" ", "")]
        [CustomInlineAutoData("String", "System.String")]
        public void FindNamespaceInAssembly_ReturnsCorrectResult(string target, string expected, WindowLocator sut, TestAssembly assembly)
        {
            var actual = sut.FindNamespaceInAssembly(assembly, target);
            Assert.Equal(expected, actual);
        }
    }
}
