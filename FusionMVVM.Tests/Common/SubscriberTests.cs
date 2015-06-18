using System;
using FusionMVVM.Common;
using Ploeh.AutoFixture;
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
            var fixture = new Fixture();
            var action = fixture.Create<Action>();
            var sut = new Subscriber(action);

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.Invoke<object>(null));
        }
    }
}
