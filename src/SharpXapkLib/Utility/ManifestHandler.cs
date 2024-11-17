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

            manifest.SplitApks?.ForEach(apk => apk.File = $"{apk.Id}.apk");
            manifest.Expansions?.ForEach(exp => exp.File = exp.InstallPath);

            return XapkPackagingTool.Common.Utility.ObjectSerialization.JsonSerializer.Serialize(manifest);
        }

        public static XapkManifest DeserializeManifest(string jsonData, string xapkPath)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                throw new ArgumentNullException(nameof(jsonData));
            if (string.IsNullOrWhiteSpace(xapkPath))
                throw new ArgumentNullException(nameof(xapkPath));

            var manifest = XapkPackagingTool.Common.Utility.ObjectSerialization.JsonSerializer
                .Deserialize<XapkManifest>(jsonData) ?? throw new InvalidOperationException("Manifest deserialization failed.");

            if (!string.IsNullOrWhiteSpace(manifest.Icon) && !manifest.Icon.Contains('>'))
                manifest.Icon = $"{xapkPath}>{manifest.Icon}";

            manifest.SplitApks?.ForEach(apk =>
            {
                if (!string.IsNullOrWhiteSpace(apk.File) && !apk.File.Contains('>'))
                    apk.File = $"{xapkPath}>{apk.File}";
            });

            manifest.Expansions?.ForEach(exp =>
            {
                if (!string.IsNullOrWhiteSpace(exp.File) && !exp.File.Contains('>'))
                    exp.File = $"{xapkPath}>{exp.File}";
            });

            return manifest;
        }
    }

}
