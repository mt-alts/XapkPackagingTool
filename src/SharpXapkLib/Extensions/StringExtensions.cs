/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Text.RegularExpressions;

namespace SharpXapkLib
{
    internal static class StringExtensions
    {
        public static string Localize(this string key)
        {
            var localizedValue = SharpXapkLib.Res.Resources.ResourceManager.GetString(
                key,
                Thread.CurrentThread.CurrentUICulture
            );

            return string.IsNullOrWhiteSpace(localizedValue) ? key : localizedValue;
        }

        public static string GetValueBetweenQuotes(this string input)
        {
            string pattern = "'(.*?)'";

            Match match = Regex.Match(input, pattern);
            if (match.Success)
                return match.Groups[1].Value;

            return string.Empty;
        }
    }
}
