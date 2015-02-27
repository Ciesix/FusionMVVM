using Ploeh.AutoFixture.Xunit;

namespace FusionMVVM.Tests
{
    public class CustomInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public CustomInlineAutoDataAttribute(params object[] values)
            : base(new CustomAutoDataAttribute(), values)
        {
        }
    }
}
