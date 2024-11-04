/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpXapkLib.Inserter;
using XapkPackagingTool.Common.Data.Model.Xapk;

namespace SharpXapkLib.Utility
{
    internal class FileGenerator
    {
        public static List<XapkInsertMap> GenerateFiles(XapkConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            var entries = new List<XapkInsertMap>();

            if (!string.IsNullOrWhiteSpace(config.Manifest.Icon))
            {
                entries.Add(
                    new XapkInsertMap(source: $"{config.Manifest.Icon}", target: "icon.png")
                );
            }

            if (config.Manifest.Expansions != null && config.Manifest.Expansions.Any())
            {
                entries.AddRange(
                    config
                        .Manifest.Expansions.Select(expansion => new XapkInsertMap(
                            source: $"{expansion.File}",
                            target: $"{expansion.InstallPath}"
                        ))
                        .ToList()
                );
            }

            if (config.Manifest.SplitApks != null && config.Manifest.SplitApks.Any())
                config
                    .Manifest.SplitApks.Select(splitApk => new XapkInsertMap(
                        source: $"{splitApk.File}",
                        target: $"{splitApk.Id}.apk"
                    ))
                    .ToList()
                    .ForEach(entry => entries.Add(entry));
            else if (!string.IsNullOrWhiteSpace(config.BaseApk))
            {
                entries.Add(
                    new XapkInsertMap(config.BaseApk, $"{config.Manifest.PackageName}.apk")
                );
            }
            return entries;
        }
    }
}
