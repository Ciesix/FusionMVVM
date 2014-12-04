using System;

namespace FusionMVVM.Common
{
    public class Levenshtein
    {
        /// <summary>
        /// Measuring the difference between the source and target string using
        /// Levenshtein distance algorithme.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int MeasureDistance(string source, string target)
        {
            var sl = source.Length;
            var tl = target.Length;
            var distance = new int[sl + 1, tl + 1];

            if (sl == 0) return tl;
            if (tl == 0) return sl;

            for (var i = 0; i <= sl; distance[i, 0] = i++) { }
            for (var j = 0; j <= tl; distance[0, j] = j++) { }

            for (var i = 1; i <= sl; i++)
            {
                for (var j = 1; j <= tl; j++)
                {
                    var cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sl, tl];
        }
    }
}
