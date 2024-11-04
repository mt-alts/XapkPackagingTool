/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace SharpXapkLib.Exceptions
{
    public class PackagingResourcesNotSpecifiedException : Exception
    {
        public PackagingResourcesNotSpecifiedException() : base()
        {
        }

        public PackagingResourcesNotSpecifiedException(string? message) : base(message)
        {
        }

        public PackagingResourcesNotSpecifiedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
