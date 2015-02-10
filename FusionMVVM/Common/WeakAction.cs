using System;

namespace FusionMVVM.Common
{
    public class WeakAction
    {
        private WeakReference _reference;
        private readonly Action _action;

        public object Target
        {
            get { return _reference != null ? _reference.Target : null; }
        }

        public bool IsAlive
        {
            get { return _reference != null && _reference.IsAlive; }
        }

        /// <summary>
        /// Initializes a new instance of the WeakAction class.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        public WeakAction(object target, Action action)
        {
            _reference = new WeakReference(target);
            _action = action;
        }

        /// <summary>
        /// Marks the internal WeakReference ready for deletion.
        /// </summary>
        public void MarkForDeletion()
        {
            _reference = null;
        }
    }
}
