using System;
using FusionMVVM.Service;
using FusionMVVM.Tests.Fakes;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service.WindowLocatorTests
{
    public class CreateWindowTests
    {
        [Theory, CustomAutoData]
        public void NullViewModelThrowsException(WindowLocator sut, FakeViewModel owner)
        {
            Assert.Throws<ArgumentNullException>(() => sut.CreateWindow(null, owner));
        }

        [Theory, CustomAutoData]
        public void ViewModelTypeNotRegisteredReturnsNull(WindowLocator sut, FakeViewModel viewModel)
        {
            var actual = sut.CreateWindow(viewModel, null);
            Assert.Null(actual);
        }

        [Theory, CustomAutoData]
        public void CreateWindowReturnsCorrectResult(WindowLocator sut, FakeViewModel viewModel)
        {
            sut.Register<FakeViewModel, FakeWindow>();
            var actual = sut.CreateWindow(viewModel, null);
            Assert.IsType<FakeWindow>(actual);
        }
    }
}
