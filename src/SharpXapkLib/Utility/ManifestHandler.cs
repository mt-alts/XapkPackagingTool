/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace SharpXapkLib.Utility
{
    internal class ManifestHandler
    {
        public static string SerializeManifest(XapkManifest manifest)
        {
            if (manifest == null)
                throw new ArgumentNullException(nameof(manifest));
            manifest.Icon = "icon.png";
            if (manifest.SplitApks != null && manifest.SplitApks.Count > 0)
                manifest.SplitApks.ForEach(apk =>
                {
                    apk.File = $"{apk.Id}.apk";
                });
            if (manifest.Expansions != null && manifest.Expansions.Count > 0)
                manifest.Expansions.ForEach(exp => exp.File = exp.InstallPath);

            return XapkPackagingTool.Common.Utility.ObjectSerialization.JsonSerializer.Serialize(
                manifest
            );
        }

        public static XapkManifest DeserializeManifest(string jsonData, string xapkPath)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                throw new ArgumentNullException(nameof(jsonData));
            if (string.IsNullOrWhiteSpace(xapkPath))
                throw new ArgumentNullException(nameof(xapkPath));

            XapkManifest manifest =
                XapkPackagingTool.Common.Utility.ObjectSerialization.JsonSerializer.Deserialize<XapkManifest>(
                    jsonData
                );

            if (manifest == null)
                throw new InvalidOperationException("Manifest deserialization failed.");

            if (!string.IsNullOrWhiteSpace(manifest.Icon) && !manifest.Icon.Contains('>'))
                manifest.Icon = $"{xapkPath}>{manifest.Icon}";

            if (manifest.SplitApks != null && manifest.SplitApks.Count > 0)
                manifest.SplitApks.ForEach(apk =>
                {
                    if (!string.IsNullOrWhiteSpace(apk.File) && !apk.File.Contains('>'))
                        apk.File = $"{xapkPath}>{apk.File}";
                });

            if (manifest.Expansions != null && manifest.Expansions.Count > 0)
                manifest.Expansions.ForEach(exp =>
                {
                    if (!string.IsNullOrWhiteSpace(exp.File) && !exp.File.Contains('>'))
                        exp.File = $"{xapkPath}>{exp.File}";
                });

            return manifest;
        }
    }
}
