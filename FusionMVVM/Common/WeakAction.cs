using System;

namespace FusionMVVM.Common
{
    public class WeakAction
    {
        private readonly WeakReference _reference;
        private readonly Action _action;

        public WeakAction(object target, Action action)
        {
            _reference = new WeakReference(target);
            _action = action;
        }
    }
}
