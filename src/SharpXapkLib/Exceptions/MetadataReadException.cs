/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace SharpXapkLib.Exceptions
{
    public class MetadataReadException : Exception
    {
        public string PackageFile { get; private set; }

        public MetadataReadException(string message, string packageFile)
            : base(message)
        {
            PackageFile = packageFile;
        }

        public MetadataReadException(string message, string packageFile, Exception? innerException)
            : base(message, innerException)
        {
            PackageFile = packageFile;
        }
    }
}
