using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FusionMVVM.Service;
using FusionMVVM.Tests.TestData;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class RegisterTests
    {
        [Fact]
        public void NullViewModelTypeThrowException()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.Register(null));
        }

        [Theory, AutoData]
        public void RegisterReturnsCorrectResult(Type viewModelType, Type viewType)
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();

            // Exercise system.
            sut.Register(viewModelType, viewType);
            var actual = sut.RegisteredTypes.Where(pair => pair.Key == viewModelType);

            // Verify outcome.
            Assert.Equal(1, actual.Count());
        }

        [Theory, AutoData]
        public void RegisterSameViewModelTypeReturnsCorrectResult(Type viewModelType)
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();

            // Exercise system.
            sut.Register(viewModelType, typeof(UserControl));
            sut.Register(viewModelType, typeof(Window));
            var actual = sut.RegisteredTypes.FirstOrDefault(pair => pair.Key == viewModelType);

            // Verify outcome.
            Assert.Equal(typeof(Window), actual.Value);
        }

        [Fact]
        public void NullViewTypeReturnsMatchingViewType()
        {
            // Fixture setup.
            var fixture = new Fixture().Customize(new WindowLocatorCustomization());
            var sut = fixture.Create<WindowLocator>();

            // Exercise system.
            var viewModelType = typeof(FooViewModel);
            sut.Register(viewModelType);
            var actual = sut.RegisteredTypes.FirstOrDefault(pair => pair.Key == viewModelType);

            // Verify outcome.
            Assert.Equal(typeof(FooView), actual.Value);
        }
    }
}
