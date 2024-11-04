/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Exceptions
{
    internal class PackageImportException : Exception
    {
        internal string Package { get; }

        public PackageImportException(string package)
            : base(string.Format("PackageImportErrorMessage".Localize(), package))
        {
            Package = package;
        }

        public PackageImportException(string package, Exception innerException)
            : base(string.Format("PackageImportErrorMessage".Localize(), package), innerException)
        {
            Package = package;
        }
    }
}
