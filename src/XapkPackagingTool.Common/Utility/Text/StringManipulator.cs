/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Utility.Text
{
    public class StringManipulator
    {
        public static string RemoveSpaces(string input)
        {
            return input.Replace(" ", string.Empty);
        }

        public static string RemoveDuplicateBackslashes(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"\\{2,}", @"\");
        }
    }
}
