/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Service.Interfaces.DataService
{
    internal interface ISplitApkService
    {
        public List<SplitApk> SplitApks { get; set; }
    }
}
