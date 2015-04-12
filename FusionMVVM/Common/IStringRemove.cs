namespace FusionMVVM.Common
{
    public interface IStringRemove
    {
        /// <summary>
        /// Removes a predefined word or text, from the given text.
        /// Supports case sensitivity by setting the ignoreCase parameter.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        string Remove(string text, bool ignoreCase = false);
    }
}
