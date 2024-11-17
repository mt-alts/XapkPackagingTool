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
                throw new ArgumentNullException(nameof(config));

            var entries = new List<XapkInsertMap>();

            if (!string.IsNullOrWhiteSpace(config.Manifest.Icon))
                entries.Add(new XapkInsertMap(config.Manifest.Icon, "icon.png"));

            if (config.Manifest.Expansions is List<Expansion> expansions && expansions.Any())
                entries.AddRange(
                    expansions.Select(expansion => new XapkInsertMap(
                        source: expansion.File,
                        target: expansion.InstallPath
                    ))
                );

            if (config.Manifest.SplitApks is List<SplitApk> splitApks && splitApks.Any())
                entries.AddRange(
                    splitApks.Select(splitApk => new XapkInsertMap(
                        source: splitApk.File,
                        target: $"{splitApk.Id}.apk"
                    ))
                );

            if (!string.IsNullOrWhiteSpace(config.BaseApk))
                entries.Add(
                    new XapkInsertMap(config.BaseApk, $"{config.Manifest.PackageName}.apk")
                );

            return entries;
        }
    }
}
