/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Windows;

namespace XapkPackagingTool
{
    internal static class StringExtensions
    {
        public static string Localize(this string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            var localizedString = Application.Current.Resources[key] as string;

            return localizedString ?? key;
        }
    }
}
