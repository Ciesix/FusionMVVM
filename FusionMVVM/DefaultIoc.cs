using System;
using System.Collections.Concurrent;
using System.Linq;
using FusionMVVM.Common;

namespace FusionMVVM
{
    internal class DefaultIoc : IContainer
    {
        private readonly ConcurrentDictionary<Type, Type> _registeredTypes = new ConcurrentDictionary<Type, Type>();
        private readonly ConcurrentDictionary<Type, object> _storedServices = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Registers a type. When Resolve is called, a new object will be create.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TType"></typeparam>
        public void RegisterType<TInterface, TType>()
        {
            var key = typeof(TInterface);

            RegisterType(key, typeof(TType));
            CleanupServices(key);
        }

        /// <summary>
        /// Registers an object as a singleton. When Resolve is called, the same object is returned.
        /// </summary>
        /// <param name="theObject"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void RegisterAsSingleton<TInterface>(TInterface theObject)
        {
            var key = typeof(TInterface);

            RegisterType(key, theObject.GetType());
            _storedServices.AddOrUpdate(key, k => theObject, (k, v) => theObject);
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

            CleanupServices(key);
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
                object instance;
                if (_storedServices.TryGetValue(key, out instance))
                {
                    // Return the stored singleton object.
                    return (TInterface)instance;
                }

                // Get the first constructor from the registered type.
                var constructor = type.GetConstructors().First();

                // Create a new instance and return it.
                var activator = Activator<TInterface>.GetActivator(constructor);
                return activator();
            }

            return default(TInterface);
        }

        /// <summary>
        /// Registers a key/type as a pair.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        private void RegisterType(Type key, Type type)
        {
            // Add the key/type or update a previous registered type, with the same key.
            _registeredTypes.AddOrUpdate(key, k => type, (k, v) => type);
        }

        /// <summary>
        /// Cleanup stored services with the matching key.
        /// </summary>
        /// <param name="key"></param>
        private void CleanupServices(Type key)
        {
            object instance;
            _storedServices.TryRemove(key, out instance);
        }
    }
}
