/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Utility.Reader
{
    internal interface IPackageReader
    {
        public XapkConfig Read(string packagePath);
    }
}
