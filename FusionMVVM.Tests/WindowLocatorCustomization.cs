using FusionMVVM.Common;
using Ploeh.AutoFixture;

namespace FusionMVVM.Tests
{
    public class WindowLocatorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<IMetric>(() => new Levenshtein());
        }
    }
}
