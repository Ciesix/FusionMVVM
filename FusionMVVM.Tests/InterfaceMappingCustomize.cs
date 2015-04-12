using System;
using FusionMVVM.Common;
using Ploeh.AutoFixture;

namespace FusionMVVM.Tests
{
    public class InterfaceMappingCustomize : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<IWindowInitiator>(() => new WindowInitiator());
            fixture.Register<IFilter<Type>>(() => new TypeEndsWithFilter());
            fixture.Register<IMetric>(() => new Levenshtein());
        }
    }
}
