using System;

namespace FusionMVVM.Common
{
    public class WeakAction<T> : WeakAction
    {
        private readonly Action<T> _action;

        public WeakAction(object target, Action<T> action)
            : base(target, null)
        {
            _action = action;
        }
    }
}
