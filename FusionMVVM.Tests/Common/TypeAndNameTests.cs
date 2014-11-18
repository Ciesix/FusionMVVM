using System;
using FusionMVVM.Common;
using FusionMVVM.Tests.Fakes;
using Xunit;

namespace FusionMVVM.Tests.Common
{
    public class TypeAndNameTests
    {
        [Fact]
        public void TypeParameterIsNull_ThrowException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TypeAndName(null, "Test"));
            Assert.Equal("type", exception.ParamName);
        }
        [Fact]
        public void TypeParameterIsValid_IsType()
        {
            var sut = new TypeAndName(typeof(IFakeDatabaseService), null);
            Assert.Equal(typeof(IFakeDatabaseService), sut.Type);
        }

        [Fact]
        public void NameParameterIsNull_Null()
        {
            var sut = new TypeAndName(typeof(IFakeDatabaseService), null);
            Assert.Null(sut.Name);
        }

        [Fact]
        public void NameParameterIsValid_NotNull()
        {
            var sut = new TypeAndName(typeof(IFakeDatabaseService), "Test");
            Assert.NotNull(sut.Name);
        }

        [Fact]
        public void Equals_NullAndValidNames_NotEqual()
        {
            var one = new TypeAndName(typeof(IFakeDatabaseService), null);
            var two = new TypeAndName(typeof(IFakeDatabaseService), "Test");
            Assert.NotEqual(one, two);
        }

        [Fact]
        public void Equals_NullAndNullNames_Equal()
        {
            var one = new TypeAndName(typeof(IFakeDatabaseService), null);
            var two = new TypeAndName(typeof(IFakeDatabaseService), null);
            Assert.Equal(one, two);
        }

        [Fact]
        public void Equals_ValidAndValidNames_Equal()
        {
            var one = new TypeAndName(typeof(IFakeDatabaseService), "Test");
            var two = new TypeAndName(typeof(IFakeDatabaseService), "Test");
            Assert.Equal(one, two);
        }
    }
}
