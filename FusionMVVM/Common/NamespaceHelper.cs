using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FusionMVVM.Common
{
    public class NamespaceHelper
    {
        /// <summary>
        /// Finds a namespace in the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string FindNamespace(Assembly assembly, string target)
        {
            var namespaces = GetAllNamespacesInAssembly(assembly);
            var foundNamespaces = namespaces.FindAll(ns => ns.Contains(target));
            var shortestDistance = int.MaxValue;
            var result = string.Empty;

            foreach (var item in foundNamespaces)
            {
                var distance = Levenshtein.MeasureDistance(item, target);
                if (distance < shortestDistance)
                {
                    // The distance is shorter then the shortest distance.
                    shortestDistance = distance;
                    result = item;
                }
            }

            return result;
        }

        /// <summary>
        /// Get all namespaces in the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static List<string> GetAllNamespacesInAssembly(Assembly assembly)
        {
            return assembly.GetTypes().Select(t => t.FullName).Distinct().ToList();
        }
    }
}
