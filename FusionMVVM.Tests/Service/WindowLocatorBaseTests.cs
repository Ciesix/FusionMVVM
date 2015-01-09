﻿using System;
using System.Collections.Generic;
using System.Linq;
using FusionMVVM.Tests.Fakes;
using Xunit;
using Xunit.Extensions;

namespace FusionMVVM.Tests.Service
{
    public class WindowLocatorBaseTests
    {
        [Fact]
        public void GetViewModelTypes_Null_ThrowsException()
        {
            var sut = new FakeWindowLocatorBase();
            Assert.Throws<ArgumentNullException>(() => sut.GetViewModelTypes(null));
        }

        [Theory]
        [PropertyData("AssemblyTypes")]
        public void GetViewModelTypes_ReturnsCorrectResult(IEnumerable<Type> assemblyTypes, int expected)
        {
            var sut = new FakeWindowLocatorBase();
            var actual = sut.GetViewModelTypes(assemblyTypes).Count();
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> AssemblyTypes
        {
            get
            {
                return new[]
                {
                    new object[] {new List<Type>(), 0},
                    new object[]
                    {
                        new List<Type>
                        {
                            typeof (FakeViewModel),
                            typeof (FakeWindowLocatorBase)
                        },
                        1
                    }
                };
            }
        }
    }
}
