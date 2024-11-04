/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace SharpXapkLib.Exceptions
{
    public class PackagingDirectoryNotSpecifiedException : Exception
    {
        public PackagingDirectoryNotSpecifiedException()
            : base() { }

        public PackagingDirectoryNotSpecifiedException(string? message)
            : base(message) { }

        public PackagingDirectoryNotSpecifiedException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
