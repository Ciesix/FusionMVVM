using FusionMVVM.Tests.Fakes;
using Xunit;

namespace FusionMVVM.Tests
{
    public class DefaultIocTests
    {
        [Fact]
        public void Resolve_WhenNothingIsRegistered_Null()
        {
            Ioc.Reset();
            var service = Ioc.Current.Resolve<IFakeDatabaseService>();
            Assert.Null(service);
        }

        [Fact]
        public void RegisterType_ResolveObjectAfterRegisterType_NotNull()
        {
            Ioc.Reset();
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>();

            var service = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.NotNull(service);
            Assert.IsType<FakeDatabaseService>(service);
        }

        [Fact]
        public void RegisterType_ResolveMultipleObjectAfterRegisterType_NotSameObject()
        {
            Ioc.Reset();
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>();

            var first = Ioc.Current.Resolve<IFakeDatabaseService>();
            var second = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.NotSame(first, second);
        }

        [Fact]
        public void Unregister_ResolveObjectAfterUnregister_Null()
        {
            Ioc.Reset();
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>();

            Ioc.Current.Unregister<IFakeDatabaseService>();
            var service = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.Null(service);
        }
    }
}
