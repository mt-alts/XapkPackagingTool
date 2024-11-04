/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Apk;

namespace AaptCommandApi
{
    public interface IAapt2
    {
        ApkMetadata DumpBadging(string apkFile);
    }
}
