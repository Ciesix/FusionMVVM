using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using FusionMVVM.Common;

namespace FusionMVVM
{
    internal class DefaultIoc : IContainer
    {
        private readonly ConcurrentDictionary<TypeAndName, Type> _registeredTypes = new ConcurrentDictionary<TypeAndName, Type>();
        private readonly ConcurrentDictionary<TypeAndName, object> _storedServices = new ConcurrentDictionary<TypeAndName, object>();

        /// <summary>
        /// Registers a type. When Resolve is called, a new object will be create.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TType"></typeparam>
        public void RegisterType<TInterface, TType>()
        {
            RegisterType(typeof(TInterface), typeof(TType), null);
        }

        /// <summary>
        /// Registers a type with a name. When Resolve is called, a new object will be create.
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TType"></typeparam>
        public void RegisterType<TInterface, TType>(string name)
        {
            RegisterType(typeof(TInterface), typeof(TType), name);
        }

        /// <summary>
        /// Registers a type. When Resolve is called, a new object will be create.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="type"></param>
        public void RegisterType(Type interfaceType, Type type)
        {
            RegisterType(interfaceType, type, null);
        }

        /// <summary>
        /// Registers a type with a name. When Resolve is called, a new object will be create.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void RegisterType(Type interfaceType, Type type, string name)
        {
            if (interfaceType == null) throw new ArgumentNullException("interfaceType");
            if (type == null) throw new ArgumentNullException("type");
            if (IsNameNullOrValid(name) == false) throw new ArgumentNullException("name");

            var typeAndName = new TypeAndName(interfaceType, name);

            // Cleanup previous stored services, with the same interface type and name.
            CleanupServices(interfaceType, name);

            // Add the typeAndName/type or update a previous registered type, with the same
            // typeAndName.
            _registeredTypes.AddOrUpdate(typeAndName, k => type, (k, v) => type);
        }

        /// <summary>
        /// Registers an object as a singleton. When Resolve is called, the same object is returned.
        /// </summary>
        /// <param name="theObject"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void RegisterAsSingleton<TInterface>(TInterface theObject)
        {
            RegisterAsSingleton(typeof(TInterface), theObject, null);
        }

        /// <summary>
        /// Registers an object as a singleton with a name. When Resolve is called, the same object is returned.
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="name"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void RegisterAsSingleton<TInterface>(TInterface theObject, string name)
        {
            RegisterAsSingleton(typeof(TInterface), theObject, name);
        }

        /// <summary>
        /// Registers an object as a lazy singleton. When Resolve is called, the same object is returned.
        /// The object will be created the first time Resolve is called.
        /// </summary>
        /// <param name="theConstructor"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void RegisterAsSingleton<TInterface>(Func<TInterface> theConstructor)
        {
            RegisterAsSingleton(typeof(TInterface), theConstructor, null);
        }

        /// <summary>
        /// Registers an object as a lazy singleton with a name. When Resolve is called, the same object is returned.
        /// The object will be created the first time Resolve is called.
        /// </summary>
        /// <param name="theConstructor"></param>
        /// <param name="name"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void RegisterAsSingleton<TInterface>(Func<TInterface> theConstructor, string name)
        {
            RegisterAsSingleton(typeof(TInterface), theConstructor, name);
        }

        /// <summary>
        /// Unregisters a service. When this method is called, everything related is removed.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        public void Unregister<TInterface>()
        {
            Unregister(typeof(TInterface), null);
        }

        /// <summary>
        /// Unregisters a service with a name. When this method is called, everything related is removed.
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TInterface"></typeparam>
        public void Unregister<TInterface>(string name)
        {
            Unregister(typeof(TInterface), null);
        }

        /// <summary>
        /// Unregisters a service with a name. When this method is called, everything related is removed.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        public void Unregister(Type interfaceType, string name)
        {
            if (interfaceType == null) throw new ArgumentNullException("interfaceType");
            if (IsNameNullOrValid(name) == false) throw new ArgumentNullException("name");

            var typeAndName = new TypeAndName(interfaceType, name);

            Type type;
            _registeredTypes.TryRemove(typeAndName, out type);

            CleanupServices(interfaceType, name);
        }

        /// <summary>
        /// Resolves the last registered service.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public TInterface Resolve<TInterface>()
        {
            return Resolve<TInterface>(null);
        }

        /// <summary>
        /// Resolves the last registered service with a matching name.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public TInterface Resolve<TInterface>(string name)
        {
            var interfaceType = typeof(TInterface);
            var typeAndName = new TypeAndName(interfaceType, name);

            Type type;
            if (_registeredTypes.TryGetValue(typeAndName, out type))
            {
                object lazyInstance;
                if (_storedServices.TryGetValue(typeAndName, out lazyInstance) && lazyInstance.GetType() == typeof(Func<TInterface>))
                {
                    // Lazy singletons must be invoked when called first time.
                    var invokedLazy = ((Func<TInterface>)lazyInstance).Invoke();
                    _storedServices.AddOrUpdate(typeAndName, k => invokedLazy, (k, v) => invokedLazy);
                }

                object instance;
                if (_storedServices.TryGetValue(typeAndName, out instance))
                {
                    // Return the stored singleton object.
                    return (TInterface)instance;
                }

                // Get the first constructor from the registered type.
                var constructor = type.GetConstructors().First();

                // Create a new instance and return it.
                var activator = Common.Activator.GetActivator(constructor);
                return (TInterface)activator();
            }

            return default(TInterface);
        }

        /// <summary>
        /// Registers an interface and object as a singleton.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="theObject"></param>
        /// <param name="name"></param>
        private void RegisterAsSingleton(Type interfaceType, object theObject, string name)
        {
            var typeAndName = new TypeAndName(interfaceType, name);

            RegisterType(interfaceType, theObject.GetType(), name);
            _storedServices.AddOrUpdate(typeAndName, k => theObject, (k, v) => theObject);
        }

        /// <summary>
        /// Cleanup stored services with the matching interface type.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        private void CleanupServices(Type interfaceType, string name)
        {
            var typeAndName = new TypeAndName(interfaceType, name);

            object instance;
            _storedServices.TryRemove(typeAndName, out instance);
        }

        /// <summary>
        /// Returns true if the name is NULL or valid. Only names with numbers
        /// and letters are valid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsNameNullOrValid(string name)
        {
            if (name == null) return true;

            // Remove all non alphanumeric characters.
            var regex = new Regex("[^0-9a-zA-Z]+");
            name = regex.Replace(name, string.Empty);

            return string.IsNullOrWhiteSpace(name) == false;
        }
    }
}
