namespace FusionMVVM.Common
{
    public interface IMetric
    {
        /// <summary>
        /// Measures the difference between the source and target string and
        /// returns the number of characters that must occur to get from
        /// source to target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        int MeasureDistance(string source, string target);
    }
}
