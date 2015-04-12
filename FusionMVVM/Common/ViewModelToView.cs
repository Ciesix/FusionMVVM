using System;
using System.Text.RegularExpressions;

namespace FusionMVVM.Common
{
    public class ViewModelToView : IStringRemove
    {
        /// <summary>
        /// Removes a predefined word or text, from the given text.
        /// Supports case sensitivity by setting the ignoreCase parameter.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public string Remove(string text, bool ignoreCase = false)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (text == string.Empty) return string.Empty;

            var option = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

            return Regex.Replace(text, "Model", string.Empty, option);
        }
    }
}
