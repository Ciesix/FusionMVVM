using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FusionMVVM.Common;

namespace FusionMVVM.Service
{
    public class EventAggregator : IEventAggregator
    {
        private readonly ConcurrentDictionary<Type, List<WeakAction>> _subscribers = new ConcurrentDictionary<Type, List<WeakAction>>();
        private readonly object _lock = new object();

        /// <summary>
        /// Subscribes the target to receive messages of type TMessage.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="target"></param>
        /// <param name="action"></param>
        public void Subscribe<TMessage>(object target, Action<TMessage> action)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (action == null) throw new ArgumentNullException("action");

            lock (_lock)
            {
                var messageType = typeof(TMessage);
                var list = _subscribers.GetOrAdd(messageType, k => new List<WeakAction>());
                list.Add(new WeakAction<TMessage>(target, action));
            }
        }

        /// <summary>
        /// Unsubscribes the message of type TMessage from the target.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="target"></param>
        public void Unsubscribe<TMessage>(object target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Publishes a message of type TMessage to all subscribers.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        public void Publish<TMessage>(TMessage message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a readonly list of WeakActions, that are subscribed to the
        /// given message type.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public IReadOnlyList<WeakAction> GetSubscribers<TMessage>()
        {
            var messageType = typeof(TMessage);
            var list = _subscribers.FirstOrDefault(pair => pair.Key == messageType).Value;
            return list ?? new List<WeakAction>();
        }
    }
}
