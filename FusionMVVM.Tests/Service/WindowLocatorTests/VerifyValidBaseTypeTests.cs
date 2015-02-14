using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FusionMVVM.Service;
using FusionMVVM.Tests.Fakes;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class VerifyValidBaseTypeTests
    {
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
