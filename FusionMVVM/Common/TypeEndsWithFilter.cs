using System;
using System.Collections.Generic;
using System.Linq;

namespace FusionMVVM.Common
{
    public class TypeEndsWithFilter : IFilter<Type>
    {
        /// <summary>
        /// Enumerate through the collection and return a new collection where
        /// the filter text matches the filter.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="enumerable"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public IEnumerable<Type> ApplyFilter(string text, IEnumerable<Type> enumerable, StringComparison comparison = StringComparison.Ordinal)
        {
            var filtered = from type in enumerable
                           where type.FullName != null && (type.IsClass && type.FullName.EndsWith(text, comparison))
                           select type;

            return filtered;
        }
    }
}
