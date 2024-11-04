/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
using Newtonsoft.Json;
using XapkPackagingTool.Common.Data.Equality;
using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;

namespace XapkPackagingTool.Common.Data.Model.Xapk
{
    public class SplitApk : ISplitApk, ICustomEquality<SplitApk>, ICloneable
    {
        [JsonProperty("file")] public string? File { get; set; }
        [JsonProperty("id")] public string? Id { get; set; }

        public SplitApk() { }

        public SplitApk(string id, string file)
        {
            this.Id = id;
            this.File = file;
        }

        public bool IsEqualTo(SplitApk other)
        {
            if (other != null)
                return File.Equals(other.File);
            return false;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
