namespace FusionMVVM
{
    public interface IContainer
    {
        /// <summary>
        /// Registers a type. When resolved a new instance will be returned.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TType"></typeparam>
        void RegisterType<TInterface, TType>();

        /// <summary>
        /// Resolves a registered object.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        TInterface Resolve<TInterface>();
    }
}
