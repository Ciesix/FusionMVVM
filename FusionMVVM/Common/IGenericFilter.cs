using System;
using System.Collections.Generic;

namespace FusionMVVM.Common
{
    public interface IFilter<T>
    {
        /// <summary>
        /// Enumerate through the collection and return a new collection where
        /// the filter text matches the filter.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="enumerable"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        IEnumerable<T> ApplyFilter(string text, IEnumerable<T> enumerable, StringComparison comparison = StringComparison.Ordinal);
    }
}
