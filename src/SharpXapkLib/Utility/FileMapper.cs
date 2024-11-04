/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpXapkLib.Inserter;

namespace SharpXapkLib.Utility
{
    internal class FileMapper : IFileMapper
    {
        private readonly List<XapkInsertMap> _insertMaps;

        public FileMapper()
        {
            _insertMaps = new List<XapkInsertMap>();
        }

        public void AddFiles(List<XapkInsertMap> insertMaps)
        {
            _insertMaps.AddRange(insertMaps);
        }

        public List<XapkInsertMap> GetUncompressedFiles()
        {
            return _insertMaps
                .Where(entry =>
                    !string.IsNullOrWhiteSpace(entry.Target) && !entry.Source.Contains(">")
                )
                .ToList();
        }

        public List<CompressedFileGroup> GroupItemsByCompressedFile()
        {
            var groups = _insertMaps
                .Where(entry => entry.Source.Contains(">"))
                .GroupBy(entry =>
                {
                    int index = entry.Source.IndexOf(">");
                    return entry.Source.Substring(0, index).Trim();
                })
                .Select(group => new CompressedFileGroup(
                    group.Key,
                    group
                        .Select(e => new XapkInsertMap(
                            e.Source.Substring(e.Source.IndexOf(">") + 1).Trim(),
                            e.Target
                        ))
                        .ToList()
                ))
                .ToList();

            return groups;
        }
    }

    class CompressedFileGroup
    {
        public string CompressedFileName { get; }
        public List<XapkInsertMap> Entries { get; }

        public CompressedFileGroup(string compressedFileName, List<XapkInsertMap> entries)
        {
            CompressedFileName =
                compressedFileName ?? throw new ArgumentNullException(nameof(compressedFileName));
            Entries = entries ?? throw new ArgumentNullException(nameof(entries));
        }
    }
}
