using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using FusionMVVM.Common;

namespace FusionMVVM
{
    public abstract class ContainerBase
    {
        protected readonly ConcurrentDictionary<TypeAndName, Type> RegisteredTypes = new ConcurrentDictionary<TypeAndName, Type>();
        protected readonly ConcurrentDictionary<TypeAndName, object> StoredServices = new ConcurrentDictionary<TypeAndName, object>();

        /// <summary>
        /// Cleanup stored services with the matching interface type.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="name"></param>
        protected virtual void CleanupServices(Type interfaceType, string name)
        {
            if (interfaceType == null) throw new ArgumentNullException("interfaceType");

            var typeAndName = new TypeAndName(interfaceType, name);

            object instance;
            StoredServices.TryRemove(typeAndName, out instance);
        }

        /// <summary>
        /// Returns true if the name is NULL or valid. Only names with numbers
        /// and letters are valid.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual bool IsNameNullOrValid(string name)
        {
            if (name == null) return true;

            // Remove all non alphanumeric characters.
            var regex = new Regex("[^0-9a-zA-Z]+");
            name = regex.Replace(name, string.Empty);

            return string.IsNullOrWhiteSpace(name) == false;
        }
    }
}
