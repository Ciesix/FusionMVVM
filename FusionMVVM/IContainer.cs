using System;

namespace FusionMVVM
{
    public interface IContainer
    {
        /// <summary>
        /// Registers a type. When Resolve is called, a new object will be create.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TType"></typeparam>
        void RegisterType<TInterface, TType>();

        /// <summary>
        /// Registers a type with a name. When Resolve is called, a new object will be create.
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TType"></typeparam>
        void RegisterType<TInterface, TType>(string name);

        /// <summary>
        /// Registers a type. When Resolve is called, a new object will be create.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="type"></param>
        void RegisterType(Type interfaceType, Type type);

        /// <summary>
        /// Registers a type with a name. When Resolve is called, a new object will be create.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        void RegisterType(Type interfaceType, Type type, string name);

        /// <summary>
        /// Registers an object as a singleton. When Resolve is called, the same object is returned.
        /// </summary>
        /// <param name="theObject"></param>
        /// <typeparam name="TInterface"></typeparam>
        void RegisterAsSingleton<TInterface>(TInterface theObject);

        /// <summary>
        /// Registers an object as a singleton with a name. When Resolve is called, the same object is returned.
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="name"></param>
        /// <typeparam name="TInterface"></typeparam>
        void RegisterAsSingleton<TInterface>(TInterface theObject, string name);

        /// <summary>
        /// Registers an object as a lazy singleton. When Resolve is called, the same object is returned.
        /// The object will be created the first time Resolve is called.
        /// </summary>
        /// <param name="theConstructor"></param>
        /// <typeparam name="TInterface"></typeparam>
        void RegisterAsSingleton<TInterface>(Func<TInterface> theConstructor);

        /// <summary>
        /// Registers an object as a lazy singleton with a name. When Resolve is called, the same object is returned.
        /// The object will be created the first time Resolve is called.
        /// </summary>
        /// <param name="theConstructor"></param>
        /// <param name="name"></param>
        /// <typeparam name="TInterface"></typeparam>
        void RegisterAsSingleton<TInterface>(Func<TInterface> theConstructor, string name);

        /// <summary>
        /// Unregisters a service. When this method is called, everything related is removed.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        void Unregister<TInterface>();

        /// <summary>
        /// Unregisters a service with a name. When this method is called, everything related is removed.
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TInterface"></typeparam>
        void Unregister<TInterface>(string name);

        /// <summary>
        /// Unregisters a service with a name. When this method is called, everything related is removed.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        void Unregister(Type interfaceType, string name);

        /// <summary>
        /// Resolves the last registered service.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        TInterface Resolve<TInterface>();

        /// <summary>
        /// Resolves the last registered service with a matching name.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        TInterface Resolve<TInterface>(string name);
    }
}
