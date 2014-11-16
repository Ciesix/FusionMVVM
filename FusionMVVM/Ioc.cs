using System;

namespace FusionMVVM
{
    public class Ioc
    {
        private static readonly object CreationLock = new object();

        #region Thread-safe singleton instance

        private static IContainer _instance;

        /// <summary>
        /// Gets the current instance of the Ioc container.
        /// </summary>
        public static IContainer Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (CreationLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DefaultIoc();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// Provides a way to override the default instance with a custom instance,
        /// for example for unit testing purposes.
        /// </summary>
        /// <param name="container"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetContainer(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _instance = container;
        }

        /// <summary>
        /// Sets the Ioc container's default (static) instance to null.
        /// </summary>
        public static void Reset()
        {
            _instance = null;
        }
    }
}
