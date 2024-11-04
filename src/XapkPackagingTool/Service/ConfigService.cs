/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Exceptions;
using XapkPackagingTool.Helper;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Utility.Reader;

namespace XapkPackagingTool.Service
{
    internal class ConfigService : IConfigService
    {
        private readonly string ASSET_PATH = PathHelper.GetFullPath(Properties.Path.Default.AssetBlankProjectTemplate);

        private readonly IPackageReader _packageReader;

        public string ConfigPath { get; set; }

        public ConfigService(IPackageReader packageReader)
        {
            _packageReader = packageReader;
        }

        public XapkConfig CreateNew()
        {
            if (!File.Exists(ASSET_PATH))
                throw new AssetLoadException(ASSET_PATH);
            var blankConfigJsonTxt = File.ReadAllText(ASSET_PATH);
            var config = Common.Utility.ObjectSerialization.JsonSerializer.Deserialize<XapkConfig>(
                blankConfigJsonTxt
            );
            return config;
        }

        public XapkConfig LoadFromDisk(string configFilePath)
        {
            if (!File.Exists(configFilePath))
                throw new FileNotFoundException(
                    string.Format("StrFileNotFoundMessage".Localize(), configFilePath)
                );

            var configFileContent = File.ReadAllText(configFilePath);
            var config = Common.Utility.ObjectSerialization.JsonSerializer.Deserialize<XapkConfig>(
                configFileContent
            );

            if (config == null)
                throw new UnableToReadConfigurationException(configFilePath);

            return config;
        }

        public XapkConfig ImportFromPackage(string package)
        {
            if (string.IsNullOrWhiteSpace(package))
                throw new FileNotFoundException(
                    string.Format("StrFileNotFoundMessage".Localize(), package)
                );
            var config = _packageReader.Read(package);
            if (config == null)
                throw new PackageImportException(package);
            return config;
        }

        public void SaveChanges(XapkConfig xapkConfig)
        {
            Save(ConfigPath, xapkConfig);
        }

        public void Save(string filePath, XapkConfig config)
        {
            if (string.IsNullOrWhiteSpace(filePath) || config == null)
                return;

            File.WriteAllText(
                filePath,
                Common.Utility.ObjectSerialization.JsonSerializer.Serialize<XapkConfig>(config)
            );
        }
    }
}
