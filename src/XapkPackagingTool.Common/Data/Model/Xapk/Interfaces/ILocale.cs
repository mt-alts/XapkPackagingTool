/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
namespace XapkPackagingTool.Common.Data.Model.Xapk.Interfaces
{
    public interface ILocale
    {
        public string? LanguageCode { get; set; }
        public string? Name { get; set; }
    }
}
