/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
using Newtonsoft.Json;
using XapkPackagingTool.Common.Data.Equality;
using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;

namespace XapkPackagingTool.Common.Data.Model.Xapk
{
    public class Expansion : IExpansion, ICustomEquality<Expansion>, ICloneable
    {
        [JsonProperty("file")] public string? File { get; set; }
        [JsonProperty("install_location")] public string? InstallLocation { get; set; }
        [JsonProperty("install_path")] public string? InstallPath { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool IsEqualTo(Expansion other)
        {
            if (other != null)
                return
                    File.Equals(other.File) &&
                    InstallLocation.Equals(other.InstallLocation) &&
                    InstallPath.Equals(other.InstallPath);
            return false;
        }
    }
}
