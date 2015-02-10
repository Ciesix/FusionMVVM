using System;

namespace FusionMVVM.Service
{
    public interface IEventAggregator
    {
        /// <summary>
        /// Subscribes the target to receive messages of type TMessage.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="target"></param>
        /// <param name="action"></param>
        void Subscribe<TMessage>(object target, Action<TMessage> action);

        /// <summary>
        /// Unsubscribes the message of type TMessage from the target.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="target"></param>
        void Unsubscribe<TMessage>(object target);

        /// <summary>
        /// Publishes a message of type TMessage to all subscribers.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        void Publish<TMessage>(TMessage message);
    }
}
