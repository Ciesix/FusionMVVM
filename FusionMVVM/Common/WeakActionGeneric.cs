using System;

namespace FusionMVVM.Common
{
    public class WeakAction<T> : WeakAction
    {
        private readonly Action<T> _action;

        /// <summary>
        /// Initializes a new instance of the WeakAction class.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        public WeakAction(object target, Action<T> action)
            : base(target, null)
        {
            _action = action;
        }
    }
}
