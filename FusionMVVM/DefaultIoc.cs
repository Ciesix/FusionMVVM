using System;
using System.Collections.Concurrent;
using System.Linq;
using FusionMVVM.Common;

namespace FusionMVVM
{
    internal class DefaultIoc : IContainer
    {
        private readonly ConcurrentDictionary<Type, Type> _registeredTypes = new ConcurrentDictionary<Type, Type>();

        /// <summary>
        /// Registers a type. When Resolve is called, a new object will be create.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TType"></typeparam>
        public void RegisterType<TInterface, TType>()
        {
            var key = typeof(TInterface);
            var value = typeof(TType);

            // Add the key/value or update a previous registered value, with the same key.
            _registeredTypes.AddOrUpdate(key, k => value, (k, v) => value);
        }

        /// <summary>
        /// Unregisters a service. When this method is called, everything related is removed.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        public void Unregister<TInterface>()
        {
            var key = typeof(TInterface);

            Type type;
            _registeredTypes.TryRemove(key, out type);
        }

        /// <summary>
        /// Resolves the last registered service.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public TInterface Resolve<TInterface>()
        {
            var key = typeof(TInterface);

            Type type;
            if (_registeredTypes.TryGetValue(key, out type))
            {
                // Get the first constructor from the registered type.
                var constructor = type.GetConstructors().First();

                // Create a new instance and return it.
                var activator = Activator<TInterface>.GetActivator(constructor);
                return activator();
            }

            return default(TInterface);
        }
    }
}
