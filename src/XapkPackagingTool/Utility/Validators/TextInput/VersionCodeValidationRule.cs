/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace XapkPackagingTool.Utility.Validators.TextInput
{
    internal class VersionCodeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if (string.IsNullOrWhiteSpace(input))
                return new ValidationResult(false, "IsFieldRequiredAndNonWhitespace".Localize());

            string pattern = @"^\d+$";
            if (!Regex.IsMatch(input, pattern))
                return new ValidationResult(false, "InvalidVersionCode".Localize());

            return ValidationResult.ValidResult;
        }
    }
}