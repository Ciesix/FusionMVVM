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
