/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Utility.ZipUtility
{
    public class FileCopyInfo
    {
        public string Source { get; set; }
        public string Target { get; set; }

        public FileCopyInfo(string source, string target)
        {
            Source = source;
            Target = target;
        }
    }
}
