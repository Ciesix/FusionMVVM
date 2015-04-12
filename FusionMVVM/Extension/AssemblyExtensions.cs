using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using FusionMVVM.Common;

namespace FusionMVVM.Extension
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Returns a type from the assembly with the closest matching name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="metric"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetType(this Assembly assembly, IMetric metric, string typeName)
        {
            if (metric == null) throw new ArgumentNullException("metric");
            if (typeName == null) throw new ArgumentNullException("typeName");

            var distinctTypes = assembly.GetTypes().Distinct();
            var types = distinctTypes.Where(type => type.Name.Contains(typeName));
            var shortestDistance = int.MaxValue;
            Type bestMatchingType = null;

            foreach (var type in types)
            {
                var distance = metric.MeasureDistance(type.Name, typeName);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestMatchingType = type;
                }
            }

            return bestMatchingType;
        }

        /// <summary>
        /// Returns a cluster consists of a set of connected assemblies.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="includeReferencedAssemblies"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<Assembly> GetAssemblyCluster(this Assembly assembly, bool includeReferencedAssemblies = false)
        {
            var assemblies = new List<Assembly> { assembly };

            if (includeReferencedAssemblies)
            {
                var referencedAssemblies = assembly.GetReferencedAssemblies();
                assemblies.AddRange(referencedAssemblies.Select(Assembly.Load));
            }

            return new ReadOnlyCollection<Assembly>(assemblies);
        }
    }
}
