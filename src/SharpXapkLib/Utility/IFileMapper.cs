/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpXapkLib.Inserter;

namespace SharpXapkLib.Utility
{
    internal interface IFileMapper
    {
        public void AddFiles(List<XapkInsertMap> insertMaps);
        public List<XapkInsertMap> GetUncompressedFiles();
        public List<CompressedFileGroup> GroupItemsByCompressedFile();

    }
}
