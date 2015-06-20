using System;
using FusionMVVM.Common;
using Xunit;

namespace FusionMVVM.Tests.Common
{
    public class SubscriberTests
    {
        [Fact]
        public void ConstructorShouldThrowExceptionWhenActionNull()
        {
            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new Subscriber(null));
        }

        [Fact]
        public void InvokeShouldThrowExceptionWhenMessageNull()
        {
            // Fixture setup.
            var action = new Action<object>(o => { });

            // Exercise system.
            var sut = new Subscriber(action);

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.Invoke<object>(null));
        }
    }
}
