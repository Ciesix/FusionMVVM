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
            var interfaceType = typeof(TInterface);

            RegisterType(interfaceType, typeof(TType));
            CleanupServices(interfaceType);
        }

        /// <summary>
        /// Registers a type. When Resolve is called, a new object will be create.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="type"></param>
        public void RegisterType(Type interfaceType, Type type)
        {
            if (interfaceType == null) throw new ArgumentNullException("interfaceType");
            if (type == null) throw new ArgumentNullException("type");

            // Add the interfaceType/type or update a previous registered type, with the same
            // interfaceType.
            _registeredTypes.AddOrUpdate(interfaceType, k => type, (k, v) => type);
        }

        /// <summary>
        /// Registers an object as a singleton. When Resolve is called, the same object is returned.
        /// </summary>
        /// <param name="theObject"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void RegisterAsSingleton<TInterface>(TInterface theObject)
        {
            var interfaceType = typeof(TInterface);

            RegisterType(interfaceType, theObject.GetType());
            _storedServices.AddOrUpdate(interfaceType, k => theObject, (k, v) => theObject);
        }

        /// <summary>
        /// Unregisters a service. When this method is called, everything related is removed.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        public void Unregister<TInterface>()
        {
            var interfaceType = typeof(TInterface);

            Type type;
            _registeredTypes.TryRemove(interfaceType, out type);

            CleanupServices(interfaceType);
        }

        /// <summary>
        /// Resolves the last registered service.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public TInterface Resolve<TInterface>()
        {
            var interfaceType = typeof(TInterface);

            Type type;
            if (_registeredTypes.TryGetValue(interfaceType, out type))
            {
                object instance;
                if (_storedServices.TryGetValue(interfaceType, out instance))
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
        /// Cleanup stored services with the matching interface type.
        /// </summary>
        /// <param name="interfaceType"></param>
        private void CleanupServices(Type interfaceType)
        {
            object instance;
            _storedServices.TryRemove(interfaceType, out instance);
        }
    }
}
