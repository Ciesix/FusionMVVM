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
            if (target == null) throw new ArgumentNullException("target");

            lock (_lock)
            {
                var messageType = typeof(TMessage);
                List<WeakAction> list;

                if (_subscribers.TryGetValue(messageType, out list))
                {
                    var weakActions = list.Where(action => action.Target == target);
                    foreach (var weakAction in weakActions)
                    {
                        weakAction.MarkForDeletion();
                    }

                    Cleanup();
                }
            }
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
        /// Cleanup all dead WeakActions and removes them from the subscribers.
        /// </summary>
        public void Cleanup()
        {
            lock (_lock)
            {
                foreach (var subscriber in _subscribers.ToList())
                {
                    foreach (var weakAction in subscriber.Value.ToList().Where(weakAction => weakAction.IsAlive == false))
                    {
                        // Removes the dead WeakAction.
                        subscriber.Value.Remove(weakAction);
                    }

                    if (subscriber.Value.Any() == false)
                    {
                        // Removes the messageType if no WeakActions are left.
                        List<WeakAction> list;
                        _subscribers.TryRemove(subscriber.Key, out list);
                    }
                }
            }
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
