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
        /// Registers a type. When Resolve is called, a new object will be create.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="type"></param>
        void RegisterType(Type interfaceType, Type type);

        /// <summary>
        /// Registers an object as a singleton. When Resolve is called, the same object is returned.
        /// </summary>
        /// <param name="theObject"></param>
        /// <typeparam name="TInterface"></typeparam>
        void RegisterAsSingleton<TInterface>(TInterface theObject);

        /// <summary>
        /// Unregisters a service. When this method is called, everything related is removed.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        void Unregister<TInterface>();

        /// <summary>
        /// Resolves the last registered service.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        TInterface Resolve<TInterface>();
    }
}
