/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Exceptions
{
    internal class UnableToReadConfigurationException : Exception
    {
        public string ConfigurationFile { get; }

        public UnableToReadConfigurationException(string configFile)
            : base(string.Format("XapkConfigFileReadErrorMessage".Localize(), configFile))
        {
            ConfigurationFile = configFile;
        }

        public UnableToReadConfigurationException(string configFile, Exception innerException)
            : base(
                string.Format("XapkConfigFileReadErrorMessage".Localize(), configFile),
                innerException
            )
        {
            ConfigurationFile = configFile;
        }
    }
}
