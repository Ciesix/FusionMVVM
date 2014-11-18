using System;
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
        public void RegisterType_InterfaceTypeParameterIsNull_ThrowException()
        {
            Ioc.Reset();
            var exception = Assert.Throws<ArgumentNullException>(() => Ioc.Current.RegisterType(null, typeof(FakeDatabaseService)));
            Assert.Equal("interfaceType", exception.ParamName);
        }

        [Fact]
        public void RegisterType_TypeParameterIsNull_ThrowException()
        {
            Ioc.Reset();
            var exception = Assert.Throws<ArgumentNullException>(() => Ioc.Current.RegisterType(typeof(IFakeDatabaseService), null));
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public void RegisterType_NameParameterIsNull_NotNull()
        {
            Ioc.Reset();
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>(null);

            var service = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.NotNull(service);
        }

        [Fact]
        public void RegisterType_NameParameterIsValid_NotNull()
        {
            Ioc.Reset();
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>("SQL");

            var service = Ioc.Current.Resolve<IFakeDatabaseService>("SQL");

            Assert.NotNull(service);
        }

        [Fact]
        public void RegisterType_ResolveObject_NotNull()
        {
            Ioc.Reset();
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>();

            var service = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.NotNull(service);
            Assert.IsType<FakeDatabaseService>(service);
        }

        [Fact]
        public void RegisterType_ResolveMultipleObject_NotSameObject()
        {
            Ioc.Reset();
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>();

            var first = Ioc.Current.Resolve<IFakeDatabaseService>();
            var second = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.NotSame(first, second);
        }

        [Fact]
        public void RegisterAsSingleton_ResolveObject_NotNull()
        {
            Ioc.Reset();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(new FakeDatabaseService());

            var service = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.NotNull(service);
            Assert.IsType<FakeDatabaseService>(service);
        }

        [Fact]
        public void RegisterAsSingleton_ResolveMultipleObject_SameObject()
        {
            Ioc.Reset();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(new FakeDatabaseService());

            var first = Ioc.Current.Resolve<IFakeDatabaseService>();
            var second = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.Same(first, second);
        }

        [Fact]
        public void RegisterAsSingleton_NameParameterIsNull_NotNull()
        {
            Ioc.Reset();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(new FakeDatabaseService(), null);

            var service = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.NotNull(service);
        }

        [Fact]
        public void RegisterAsSingleton_NameParameterIsValid_NotNull()
        {
            Ioc.Reset();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(new FakeDatabaseService(), "SQL");

            var service = Ioc.Current.Resolve<IFakeDatabaseService>("SQL");

            Assert.NotNull(service);
        }

        [Fact]
        public void RegisterAsSingletonLazy_ResolveObject_NotNull()
        {
            Ioc.Reset();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(() => new FakeDatabaseService());

            var service = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.NotNull(service);
            Assert.IsType<FakeDatabaseService>(service);
        }

        [Fact]
        public void RegisterAsSingletonLazy_ResolveMultipleObject_SameObject()
        {
            Ioc.Reset();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(() => new FakeDatabaseService());

            var first = Ioc.Current.Resolve<IFakeDatabaseService>();
            var second = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.Same(first, second);
        }

        [Fact]
        public void Unregister_ResolveObject_Null()
        {
            Ioc.Reset();
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>();

            Ioc.Current.Unregister<IFakeDatabaseService>();
            var service = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.Null(service);
        }
    }
}
