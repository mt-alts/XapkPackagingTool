/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Service.Interfaces.DataService
{
    interface ILocaleService
    {
        public List<Locale> Locales { get; set; }
        public string PackageGlobalName { get; set; }
    }
}
