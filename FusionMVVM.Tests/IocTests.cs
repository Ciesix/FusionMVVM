using System;
using Moq;
using Xunit;

namespace FusionMVVM.Tests
{
    public class IocTests
    {
        [Fact]
        public void SetContainer_WhenContainerParameterIsNull_ThrowException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Ioc.SetContainer(null));
            Assert.Equal("container", exception.ParamName);
        }

        [Fact]
        public void SetContainer_WhenContainerParameterIsValid_SetInstance()
        {
            var mockContainer = new Mock<IContainer>();

            var container = mockContainer.Object;
            Ioc.SetContainer(container);

            Assert.Same(container, Ioc.Current);
        }

        [Fact]
        public void Reset_WhenMethodCalled_SetInstanceToDefault()
        {
            var previous = Ioc.Current;
            Ioc.Reset();
            Assert.NotSame(previous, Ioc.Current);
        }
    }
}
