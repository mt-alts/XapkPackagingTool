/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;

namespace XapkPackagingTool.Service.Interfaces.DataService
{
    interface IManifestService
    {
        public IXapkManifest Manifest { get; set; }
    }
}
