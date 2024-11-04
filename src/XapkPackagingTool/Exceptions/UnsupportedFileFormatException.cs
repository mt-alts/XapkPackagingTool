/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Exceptions
{
    internal class UnsupportedFileFormatException : Exception
    {
        public string FileFormat { get; }

        public UnsupportedFileFormatException(string fileFormat)
            : base(
                string.Format(
                    "StrUnsupportedFileFormat".Localize(),
                    NormalizeFileFormat(fileFormat)
                )
            )
        {
            FileFormat = NormalizeFileFormat(fileFormat);
        }

        public UnsupportedFileFormatException(string fileFormat, string message)
            : base(message)
        {
            FileFormat = NormalizeFileFormat(fileFormat);
        }

        public UnsupportedFileFormatException(
            string fileFormat,
            string message,
            Exception innerException
        )
            : base(message, innerException)
        {
            FileFormat = NormalizeFileFormat(fileFormat);
        }

        private static string NormalizeFileFormat(string fileFormat) =>
            fileFormat.TrimStart('.').ToUpperInvariant();
    }
}
