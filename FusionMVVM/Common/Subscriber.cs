using System;

namespace FusionMVVM.Common
{
    public class Subscriber
    {
        /// <summary>
        /// Initializes a new instance of the Subscriber class.
        /// </summary>
        /// <param name="action"></param>
        public Subscriber(Delegate action)
        {
            if (action == null) throw new ArgumentNullException("action");
        }

        /// <summary>
        /// Invokes the action associated with the subscriber.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="message"></param>
        public void Invoke<TEvent>(TEvent message)
        {
            if (message == null) throw new ArgumentNullException("message");
        }
    }
}
