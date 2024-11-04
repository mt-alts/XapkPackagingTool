/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Service.Interfaces
{
    internal interface IConfigService
    {
        public string ConfigPath { get; set; }
        public XapkConfig LoadFromDisk(string projectFilePath);
        public XapkConfig ImportFromPackage(string package);
        public XapkConfig CreateNew();
        public void SaveChanges(XapkConfig config);
        public void Save(string filePath, XapkConfig config);
    }
}
