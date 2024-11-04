/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Enums;

namespace XapkPackagingTool.Service.Interfaces.DataService
{
    internal interface IApkVariantService : ISplitApkService, IMonolithicApkService
    {
        public ApkVariantSpecies VariantSpecies { get; set; }
    }
}