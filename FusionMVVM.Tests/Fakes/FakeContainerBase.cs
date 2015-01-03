using System;
using FusionMVVM.Common;

namespace FusionMVVM.Tests.Fakes
{
    public class FakeContainerBase : ContainerBase
    {
        public int CountStoredServices
        {
            get { return StoredServices.Count; }
        }

        public new void CleanupServices(Type interfaceType, string name)
        {
            base.CleanupServices(interfaceType, name);
        }

        public new bool IsNameNullOrValid(string name)
        {
            return base.IsNameNullOrValid(name);
        }

        public void AddStoredServices(Type interfaceType, object theObject, string name)
        {
            var typeAndName = new TypeAndName(interfaceType, name);
            StoredServices.TryAdd(typeAndName, theObject);
        }
    }
}
