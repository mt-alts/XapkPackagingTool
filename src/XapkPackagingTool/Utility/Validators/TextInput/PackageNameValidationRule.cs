/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace XapkPackagingTool.Utility.Validators.TextInput
{
    class PackageNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if (string.IsNullOrWhiteSpace(input))
                return new ValidationResult(false, "IsFieldRequiredAndNonWhitespace".Localize());

            string pattern = @"^[a-zA-Z][a-zA-Z0-9]*([.][a-zA-Z][a-zA-Z0-9]*)+$";
            if (!Regex.IsMatch(input, pattern))
                return new ValidationResult(false, "InvalidPackageName".Localize());

            return ValidationResult.ValidResult;
        }
    }
}
