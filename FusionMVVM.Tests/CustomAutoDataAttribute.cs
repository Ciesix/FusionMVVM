using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace FusionMVVM.Tests
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute()
            : base(new Fixture().Customize(new InterfaceMappingCustomize()))
        {
        }
    }
}
