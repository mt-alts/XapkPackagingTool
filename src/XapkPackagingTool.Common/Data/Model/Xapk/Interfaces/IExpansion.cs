/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
namespace XapkPackagingTool.Common.Data.Model.Xapk.Interfaces
{
    public interface IExpansion
    {
        public string? File { get; set; }
        public string? InstallLocation { get; set; }
        public string? InstallPath { get; set; }
    }
}
