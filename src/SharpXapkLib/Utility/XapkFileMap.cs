/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpXapkLib.Inserter;

namespace SharpXapkLib.Utility
{
    internal class XapkFileMap
    {
        public List<XapkInsertMap> Uncompressed { get; }
        public List<CompressedFileGroup> Compressed { get; }

        public XapkFileMap(List<XapkInsertMap> uncompressed, List<CompressedFileGroup> compressed)
        {
            Uncompressed = uncompressed;
            Compressed = compressed;
        }
    }
}
