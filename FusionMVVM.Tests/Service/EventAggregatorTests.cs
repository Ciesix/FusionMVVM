using System;
using System.Linq;
using FusionMVVM.Service;
using Ploeh.AutoFixture;
using Xunit;

namespace FusionMVVM.Tests.Service
{
    public class EventAggregatorTests
    {
        [Fact]
        public void SubscribeShouldThrowExceptionWhenActionNull()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var sut = fixture.Create<EventAggregator>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.Subscribe<object>(null));
        }

        [Fact]
        public void SubscribeWithoutTokenShouldReturnCorrect()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var sut = fixture.Create<EventAggregator>();

            // Exercise system.
            sut.Subscribe<object>(o => { });
            var actual = sut.Subscribers.First().Value.Single();

            // Verify outcome.
            Assert.NotNull(actual.Subscriber);
        }

        [Fact]
        public void SubscribeWithTokenShouldReturnCorrect()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var sut = fixture.Create<EventAggregator>();
            var token = fixture.Create<string>();

            // Exercise system.
            sut.Subscribe<object>(o => { }, token);
            var actual = sut.Subscribers.First().Value.Single();

            // Verify outcome.
            Assert.Equal(token, actual.Token);
        }

        [Fact]
        public void PublishShouldThrowExceptionWhenMessageNull()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var sut = fixture.Create<EventAggregator>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => sut.Publish<object>(null));
        }

        [Fact(Skip = "Write tests for the Subscriber class first.")]
        public void PublishWithoutTokenShouldReturnCorrect()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var sut = fixture.Create<EventAggregator>();
            var message = fixture.Create<object>();
            var isInvoked = false;

            // Exercise system.
            sut.Subscribe<object>(o => { isInvoked = true; });
            sut.Publish(message);

            // Verify outcome.
            Assert.True(isInvoked);
        }
    }
}
