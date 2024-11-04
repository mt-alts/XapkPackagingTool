/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace SharpXapkLib.Exceptions
{
    internal class MetadataConvertException : Exception
    {
        public MetadataConvertException(string message)
            : base(message) { }

        public MetadataConvertException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
