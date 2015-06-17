using System;

namespace FusionMVVM.Common
{
    public class SubscriberAndToken
    {
        public Subscriber Subscriber { get; private set; }
        public object Token { get; private set; }

        /// <summary>
        /// Initializes a new instance of the SubscriberAndToken class.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="token"></param>
        public SubscriberAndToken(Subscriber subscriber, object token)
        {
            if (subscriber == null) throw new ArgumentNullException("subscriber");

            Subscriber = subscriber;
            Token = token;
        }
    }
}
