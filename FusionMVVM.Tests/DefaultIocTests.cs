using System;
using FusionMVVM.Tests.Fakes;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests
{
    public class DefaultIocTests
    {
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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(@"!@#£¤$&/\=?+-_*()[]{}")]
        public void RegisterType_WhenNameParameterIsInvalid_ThrowException(string name)
        {
            Ioc.Reset();
            var exception = Assert.Throws<ArgumentNullException>(() => Ioc.Current.RegisterType(typeof(IFakeDatabaseService), typeof(FakeDatabaseService), name));
            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("0123456789")]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public void RegisterType_WhenNameParameterIsValid_NotNull(string name)
        {
            Ioc.Reset();
            Ioc.Current.RegisterType(typeof(IFakeDatabaseService), typeof(FakeDatabaseService), name);

            var service = Ioc.Current.Resolve<IFakeDatabaseService>(name);

            Assert.NotNull(service);
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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(@"!@#£¤$&/\=?+-_*()[]{}")]
        public void RegisterAsSingleton_WhenNameParameterIsInvalid_ThrowException(string name)
        {
            Ioc.Reset();
            var exception = Assert.Throws<ArgumentNullException>(() => Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(new FakeDatabaseService(), name));
            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("0123456789")]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public void RegisterAsSingleton_WhenNameParameterIsValid_NotNull(string name)
        {
            Ioc.Reset();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(new FakeDatabaseService(), name);

            var service = Ioc.Current.Resolve<IFakeDatabaseService>(name);

            Assert.NotNull(service);
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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(@"!@#£¤$&/\=?+-_*()[]{}")]
        public void RegisterAsSingletonLazy_WhenNameParameterIsInvalid_ThrowException(string name)
        {
            Ioc.Reset();
            var exception = Assert.Throws<ArgumentNullException>(() => Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(() => new FakeDatabaseService(), name));
            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("0123456789")]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public void RegisterAsSingletonLazy_WhenNameParameterIsValid_NotNull(string name)
        {
            Ioc.Reset();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(() => new FakeDatabaseService(), name);

            var service = Ioc.Current.Resolve<IFakeDatabaseService>(name);

            Assert.NotNull(service);
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
        public void Unregister_InterfaceTypeParameterIsNull_ThrowException()
        {
            Ioc.Reset();
            var exception = Assert.Throws<ArgumentNullException>(() => Ioc.Current.Unregister(null, null));
            Assert.Equal("interfaceType", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(@"!@#£¤$&/\=?+-_*()[]{}")]
        public void Unregister_WhenNameParameterIsInvalid_ThrowException(string name)
        {
            Ioc.Reset();
            var exception = Assert.Throws<ArgumentNullException>(() => Ioc.Current.Unregister(typeof(IFakeDatabaseService), name));
            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("0123456789")]
        [InlineData("abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public void Unregister_WhenNameParameterIsValid_Null(string name)
        {
            Ioc.Reset();
            Ioc.Current.Unregister(typeof(IFakeDatabaseService), name);

            var service = Ioc.Current.Resolve<IFakeDatabaseService>(name);

            Assert.Null(service);
        }

        [Fact]
        public void Resolve_WhenNothingIsRegistered_Null()
        {
            Ioc.Reset();
            var service = Ioc.Current.Resolve<IFakeDatabaseService>();
            Assert.Null(service);
        }

        [Fact]
        public void RegisterType_LastRegisteredWins_NotSameObjects()
        {
            Ioc.Reset();
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(new FakeDatabaseService());
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>();

            var first = Ioc.Current.Resolve<IFakeDatabaseService>();
            var second = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.NotSame(first, second);
        }

        [Fact]
        public void RegisterAsSingleton_LastRegisteredWins_SameObjects()
        {
            Ioc.Reset();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(new FakeDatabaseService());
            Ioc.Current.RegisterType<IFakeDatabaseService, FakeDatabaseService>();
            Ioc.Current.RegisterAsSingleton<IFakeDatabaseService>(new FakeDatabaseService());

            var first = Ioc.Current.Resolve<IFakeDatabaseService>();
            var second = Ioc.Current.Resolve<IFakeDatabaseService>();

            Assert.Same(first, second);
        }
    }
}
