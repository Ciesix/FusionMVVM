using FusionMVVM.Common;
using Ploeh.AutoFixture;

namespace FusionMVVM.Tests
{
    public class InterfaceMappingCustomize : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<IWindowInitiator>(() => new WindowInitiator());
            fixture.Register<ITypeFilter>(() => new EndsWithTypeFilter(string.Empty));
        }
    }
}
