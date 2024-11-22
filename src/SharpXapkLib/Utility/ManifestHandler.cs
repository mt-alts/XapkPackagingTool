/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace SharpXapkLib.Utility
{
    public static class ManifestHandler
    {
        /// <summary>
        /// Serializes a given XapkManifest object into a JSON string after processing its fields.
        /// </summary>
        /// <param name="manifest">The XapkManifest object to serialize.</param>
        /// <returns>A JSON string representation of the manifest.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the manifest is null.</exception>
        public static string SerializeManifest(XapkManifest manifest)
        {
            if (manifest == null)
                throw new ArgumentNullException(nameof(manifest));

            // Process the manifest to normalize fields before serialization.
            ProcessManifest(manifest);

            // Serialize the manifest to JSON.
            return XapkPackagingTool.Common.Utility.ObjectSerialization.JsonSerializer.Serialize(manifest);
        }

        /// <summary>
        /// Deserializes a JSON string into a XapkManifest object and processes its fields
        /// to adjust file paths relative to the specified XAPK path.
        /// </summary>
        /// <param name="jsonData">The JSON data representing the manifest.</param>
        /// <param name="xapkPath">The root path of the XAPK package for resolving file paths.</param>
        /// <returns>An XapkManifest object populated with the deserialized data.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if jsonData or xapkPath is null or contains only whitespace.
        /// </exception>
        /// <exception cref="InvalidOperationException">Thrown if deserialization fails.</exception>
        public static XapkManifest DeserializeManifest(string jsonData, string xapkPath)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                throw new ArgumentNullException(nameof(jsonData));
            if (string.IsNullOrWhiteSpace(xapkPath))
                throw new ArgumentNullException(nameof(xapkPath));

            // Deserialize the JSON data into a XapkManifest object.
            var manifest = XapkPackagingTool.Common.Utility.ObjectSerialization.JsonSerializer
                .Deserialize<XapkManifest>(jsonData) ??
                throw new InvalidOperationException("Manifest deserialization failed.");

            // Process the deserialized manifest to adjust file paths.
            ProcessDeserializedManifest(manifest, xapkPath);

            return manifest;
        }

        /// <summary>
        /// Processes a XapkManifest object to normalize its fields, such as file paths and configurations.
        /// </summary>
        /// <param name="manifest">The manifest to process.</param>
        private static void ProcessManifest(XapkManifest manifest)
        {
            // Set Icon to "icon.png" if it's not already specified or invalid.
            manifest.Icon = string.IsNullOrWhiteSpace(manifest.Icon) ? null : "icon.png";

            // Update SplitApks file names and adjust SplitConfigs.
            manifest.SplitApks?.ForEach(apk => apk.File = $"{apk.Id}.apk");
            manifest.SplitConfigs = manifest.SplitApks?.Any() == true
                ? manifest.SplitConfigs?.Except(new[] { "base" }).ToList()
                : null;

            // Nullify collections if empty to avoid unnecessary serialization.
            manifest.SplitApks = manifest.SplitApks?.Any() == true ? manifest.SplitApks : null;
            manifest.Expansions?.ForEach(exp => exp.File = exp.InstallPath);
            manifest.Expansions = manifest.Expansions?.Any() == true ? manifest.Expansions : null;
            manifest.Locales = manifest.Locales?.Any() == true ? manifest.Locales : null;
            manifest.Permissions = manifest.Permissions?.Any() == true ? manifest.Permissions : null;
        }

        /// <summary>
        /// Processes a deserialized XapkManifest object to adjust file paths relative to the XAPK path.
        /// </summary>
        /// <param name="manifest">The deserialized manifest object.</param>
        /// <param name="xapkPath">The root path of the XAPK package.</param>
        private static void ProcessDeserializedManifest(XapkManifest manifest, string xapkPath)
        {
            // Adjust Icon path to include XAPK path if necessary.
            if (!string.IsNullOrWhiteSpace(manifest.Icon) && !manifest.Icon.Contains('>'))
                manifest.Icon = $"{xapkPath}>{manifest.Icon}";

            // Adjust SplitApks file paths to include XAPK path if necessary.
            manifest.SplitApks?.ForEach(apk =>
            {
                if (!string.IsNullOrWhiteSpace(apk.File) && !apk.File.Contains('>'))
                    apk.File = $"{xapkPath}>{apk.File}";
            });

            // Adjust Expansions file paths to include XAPK path if necessary.
            manifest.Expansions?.ForEach(exp =>
            {
                if (!string.IsNullOrWhiteSpace(exp.File) && !exp.File.Contains('>'))
                    exp.File = $"{xapkPath}>{exp.File}";
            });
        }
    }
}
