/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace SharpXapkLib.Exceptions
{
    internal class InsertException : Exception
    {
        public string FileSource { get; set; }
        public string Target { get; set; }

        public InsertException(string fileSource, string target)
        {
            FileSource = fileSource;
            Target = target;
        }
    }
}
