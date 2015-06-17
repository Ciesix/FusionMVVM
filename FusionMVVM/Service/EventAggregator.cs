using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FusionMVVM.Common;

namespace FusionMVVM.Service
{
    public class EventAggregator : IEventAggregator
    {
        private readonly ConcurrentDictionary<Type, List<SubscriberAndToken>> _subscribers = new ConcurrentDictionary<Type, List<SubscriberAndToken>>();

        public IReadOnlyDictionary<Type, List<SubscriberAndToken>> Subscribers
        {
            get { return new ReadOnlyDictionary<Type, List<SubscriberAndToken>>(_subscribers); }
        }

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
        public void Subscribe<TEvent>(Action<TEvent> action, object token = null)
        {
            if (action == null) throw new ArgumentNullException("action");

            var eventType = typeof(TEvent);
            var list = _subscribers.GetOrAdd(eventType, new List<SubscriberAndToken>());

            lock (list)
            {
                var subscriber = new Subscriber(action);
                list.Add(new SubscriberAndToken(subscriber, token));
            }
        }

        public void Unsubscribe<TEvent>(Action<TEvent> action, object token = null)
        {
        }

        public void Publish<TEvent>(TEvent message, object token = null)
        {
        }

        public void RequestCleanup()
        {
        }

        public void Cleanup()
        {
        }

        public void CleanupList(IDictionary<Type, List<SubscriberAndToken>> lists)
        {
        }
    }
}
