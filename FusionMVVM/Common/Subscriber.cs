using System;
using System.Windows.Threading;

namespace FusionMVVM.Common
{
    public class Subscriber
    {
        private readonly WeakReference _reference;
        private readonly Delegate _method;

        public bool IsAlive
        {
            get
            {
                if (IsStatic == false)
                {
                    return _reference != null && _reference.IsAlive;
                }

                return _reference == null || _reference.IsAlive;
            }
        }

        public bool IsStatic
        {
            get { return _reference == null || _method.Method.IsStatic; }
        }

        /// <summary>
        /// Initializes a new instance of the Subscriber class.
        /// </summary>
        /// <param name="action"></param>
        public Subscriber(Delegate action)
            : this(action == null ? null : action.Target, action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Subscriber class.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        public Subscriber(object target, Delegate action)
        {
            if (action == null) throw new ArgumentNullException("action");

            if (target != null)
            {
                // Capture the target in a WeakReference.
                _reference = new WeakReference(target);

                // Construct a new delegate that does not have a target.
                var messageType = action.Method.GetParameters()[0].ParameterType;
                var delegateType = typeof(Action<,>).MakeGenericType(target.GetType(), messageType);
                _method = Delegate.CreateDelegate(delegateType, action.Method);
            }
            else
            {
                // Method is static. Hold a strong reference to the delegate.
                _method = action;
            }
        }

        /// <summary>
        /// Invokes the action associated with the subscriber.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="message"></param>
        public bool Invoke<TEvent>(TEvent message)
        {
            if (message == null) throw new ArgumentNullException("message");

            object target = null;

            if (_reference != null) target = _reference.Target;
            if (IsAlive == false) return false;

            if (target != null && IsStatic == false)
            {
                // Normal method to invoke.
                _method.DynamicInvoke(target, message);
            }
            else
            {
                // Static method to invoke.
                _method.DynamicInvoke(message);
            }

            return true;
        }

        /// <summary>
        /// Invokes the action associated with the subscriber on the main thread.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InvokeOnMain<TEvent>(TEvent message)
        {
            var isInvoked = false;

            if (Dispatcher.CurrentDispatcher.CheckAccess() == false)
            {
                // Invokes the action in the main thread.
                Dispatcher.CurrentDispatcher.Invoke(() => isInvoked = Invoke(message));
            }
            else
            {
                // Just invoke. We are already on the main thread.
                isInvoked = Invoke(message);
            }

            return isInvoked;
        }
    }
}
