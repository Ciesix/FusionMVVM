using System;

namespace FusionMVVM.Service
{
    public interface IEventAggregator
    {
        /// <summary>
        /// Subscribes a recipient for a type of event TEvent.
        /// The action parameter will be executed when a corresponding message
        /// is sent, with a matching token.
        /// Registering a recipient does not create a hard reference to it,
        /// so if this recipient is deleted, no memory leak is caused.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="action"></param>
        /// <param name="token"></param>
        void Subscribe<TEvent>(Action<TEvent> action, object token = null);

        /// <summary>
        /// Publishes the message to subscribers. The message will reach all
        /// subscribers that registered for this message type and token.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="message"></param>
        /// <param name="token"></param>
        void Publish<TEvent>(TEvent message, object token = null);
    }
}
