using System;
using FusionMVVM.Common;
using Ploeh.AutoFixture;
using Xunit;

namespace FusionMVVM.Tests.Common
{
    public class SubscriberAndTokenTests
    {
        [Fact]
        public void ConstructorShouldThrowExceptionWhenSubscriberNull()
        {
            // Fixture setup.
            var fixture = new Fixture();
            var token = fixture.Create<object>();

            // Verify outcome.
            Assert.Throws<ArgumentNullException>(() => new SubscriberAndToken(null, token));
        }
    }
}
