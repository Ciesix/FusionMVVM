using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using FusionMVVM.Service;
using FusionMVVM.Tests.Fakes;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service
{
    public class WindowLocatorTests
    {
        [Theory, AutoData]
        public void Register_WhenViewModelType_IsNull(WindowLocator sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Register(null));
        }

        [Theory, AutoData]
        public void GetBaseName_WhenViewModel_IsNull(WindowLocator sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.GetBaseName(null));
        }

        [Theory]
        [InlineAutoData("", "")]
        [InlineAutoData("DefaultViewModel", "Default")]
        [InlineAutoData("lowercaseviewmodel", "lowercase")]
        [InlineAutoData("DoubleViewModelViewModel", "Double")]
        public void GetBaseName_ReturnsCorrectResult(string viewModel, string expected, WindowLocator sut)
        {
            var actual = sut.GetBaseName(viewModel);
            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void GetDistinctTypes_WhenTypes_IsNull(WindowLocator sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.GetDistinctTypes(null));
        }

        [Theory, AutoData]
        public void GetDistinctTypes_WhenTypesAreEmpty_ReturnEmpty(WindowLocator sut)
        {
            var types = new List<Type>();
            var actual = sut.GetDistinctTypes(types).ToList();
            Assert.Equal(0, actual.Count);
        }

        [Theory, AutoData]
        public void GetDistinctTypes_WhenResult_NotEmpty(WindowLocator sut)
        {
            var fixture = new Fixture();
            var types = fixture.CreateMany<Type>(5);

            var actual = sut.GetDistinctTypes(types).ToList();

            Assert.Equal(1, actual.Count);
        }

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

        [Theory, AutoData]
        public void ConvertNameToType_WhenTypeName_IsNull(WindowLocator sut, Assembly assembly)
        {
            Assert.Throws<ArgumentNullException>(() => sut.ConvertNameToType(null, assembly));
        }

        [Theory, AutoData]
        public void ConvertNameToType_WhenAssembly_IsNull(WindowLocator sut, string typeName)
        {
            Assert.Throws<ArgumentNullException>(() => sut.ConvertNameToType(typeName, null));
        }

        [Theory]
        [InlineAutoData("", null)]
        [InlineAutoData("Ploeh.AutoFixture.Fixture", typeof(Fixture))]
        public void ConvertNameToType_ReturnsCorrectResult(string typeName, Type expected, WindowLocator sut, Assembly assembly)
        {
            var actual = sut.ConvertNameToType(typeName, assembly);
            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void VerifyValidBaseType_WhenCurrentType_IsNull(WindowLocator sut, IEnumerable<Type> validTypes)
        {
            Assert.Throws<ArgumentNullException>(() => sut.VerifyValidBaseType(null, validTypes));
        }

        [Theory, AutoData]
        public void VerifyValidBaseType_WhenValidTypes_IsNull(WindowLocator sut, Type type)
        {
            Assert.Throws<ArgumentNullException>(() => sut.VerifyValidBaseType(type, null));
        }

        [Theory]
        [InlineAutoData(typeof(FakeWindow), true)]
        [InlineAutoData(typeof(FakeCustomWindow), true)]
        [InlineAutoData(typeof(FakeUserControl), true)]
        [InlineAutoData(typeof(FrameworkElement), false)]
        [InlineAutoData(typeof(object), false)]
        public void VerifyValidBaseType_ReturnsCorrectResult(Type currentType, bool expected, WindowLocator sut)
        {
            var validTypes = new List<Type> { typeof(Window), typeof(UserControl) };
            var actual = sut.VerifyValidBaseType(currentType, validTypes);
            Assert.Equal(expected, actual);
        }
    }
}
