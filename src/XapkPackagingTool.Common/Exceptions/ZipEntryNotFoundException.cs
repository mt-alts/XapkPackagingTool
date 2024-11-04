/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Exceptions
{
    public class ZipEntryNotFoundException : Exception
    {
        public string ZipFile { get; }
        public string ZipEntry { get; }

        public ZipEntryNotFoundException(string ZipFile, string ZipEntry)
        {
            this.ZipFile = ZipFile;
            this.ZipEntry = ZipEntry;
        }
    }
}
