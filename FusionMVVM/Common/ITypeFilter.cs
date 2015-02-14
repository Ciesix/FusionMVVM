using System;
using System.Collections.Generic;

namespace FusionMVVM.Common
{
    public interface ITypeFilter
    {
        /// <summary>
        /// Applies the filter and returns the filtered collection of types.
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        IEnumerable<Type> ApplyFilter(IEnumerable<Type> enumerable);
    }
}
