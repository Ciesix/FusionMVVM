using System;
using System.Windows;
using FusionMVVM.Common;
using FusionMVVM.Tests.Fakes;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Common.WindowInitiatorTests
{
    public class InitializeTests
    {
        [Theory, CustomAutoData]
        public void NullWindowTypeThrowsException(WindowInitiator sut, object dataContext, Mock<Window> mockOwnerWindow)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Initialize(null, dataContext, mockOwnerWindow.Object));
        }

        [Theory, CustomAutoData]
        public void WindowTypeIsCorrect(WindowInitiator sut)
        {
            var windowType = typeof(FakeWindow);
            var actual = sut.Initialize(windowType, null, null);
            Assert.IsType<FakeWindow>(actual);
        }

        [Theory, CustomAutoData]
        public void WindowHasCorrectDataContext(WindowInitiator sut, object dataContext)
        {
            var windowType = typeof(FakeWindow);
            var actual = sut.Initialize(windowType, dataContext, null);
            Assert.Same(dataContext, actual.DataContext);
        }

        [Theory, CustomAutoData]
        public void WindowHasCorrectOwner(WindowInitiator sut)
        {
            var windowType = typeof(FakeWindow);
            var ownerWindow = new FakeWindow { WindowState = WindowState.Minimized };
            ownerWindow.Show();

            var actual = sut.Initialize(windowType, null, ownerWindow);

            Assert.Same(ownerWindow, actual.Owner);
        }
    }
}
