using System;

namespace FusionMVVM.Common
{
    public class Subscriber
    {
        public Subscriber(Delegate action)
        {
        }

        public void Invoke<TEvent>(TEvent message)
        {
        }
    }
}
