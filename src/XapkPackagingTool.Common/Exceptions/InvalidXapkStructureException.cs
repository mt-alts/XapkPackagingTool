/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Exceptions
{
    public class InvalidXapkStructureException : Exception
    {
        public InvalidXapkStructureException(string message) : base(message)
        {
        }

        public InvalidXapkStructureException(string xapkFilePath, string message) : base(message)
        {
        }
    }
}
