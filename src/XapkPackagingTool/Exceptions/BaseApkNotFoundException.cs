/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Exceptions
{
    internal class BaseApkNotFoundException : Exception
    {
        public BaseApkNotFoundException()
            : base() { }

        public BaseApkNotFoundException(string? message)
            : base(message) { }

        public BaseApkNotFoundException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
