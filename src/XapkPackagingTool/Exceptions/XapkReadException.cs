/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Exceptions
{
    internal class PackageReadException : Exception
    {
        public PackageReadException(string message) : base(message) { }
    }
}
