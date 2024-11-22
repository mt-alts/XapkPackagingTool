/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
using XapkPackagingTool.Common.Data.Equality;
using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;

namespace XapkPackagingTool.Common.Data.Model.Xapk
{
    public class Locale : ILocale, ICloneable, ICustomEquality<Locale>
    {
        public string? LanguageCode { get; set; }
        public string? Name { get; set; }

        public bool IsEqualTo(Locale other)
        {
            if (other != null)
                return LanguageCode.Equals(other.LanguageCode);
            return false;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
