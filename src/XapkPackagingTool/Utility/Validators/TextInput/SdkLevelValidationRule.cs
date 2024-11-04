/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Configuration;
using System.Globalization;
using System.Windows.Controls;

namespace XapkPackagingTool.Utility.Validators.TextInput
{
    public class SdkLevelValidationRule : ValidationRule
    {
        private static readonly int MIN_SDK_LEVEL = int.Parse(ConfigurationManager.AppSettings["MinSdkVersion"]);
        private static readonly int LATEST_SDK_LEVEL = int.Parse(ConfigurationManager.AppSettings["LatestSdkVersion"]);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;
            int sdkLevel;
            int.TryParse(text, out sdkLevel);
            if (!(sdkLevel >= MIN_SDK_LEVEL && sdkLevel <= LATEST_SDK_LEVEL))
                return new ValidationResult(false, string.Format("InvalidSdkLevel".Localize(), MIN_SDK_LEVEL, LATEST_SDK_LEVEL));
            return ValidationResult.ValidResult;
        }
    }
}