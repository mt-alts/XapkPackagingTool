/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace SharpXapkLib.Inserter
{
    internal sealed class XapkInsertMap
    {
        public string Source { get; }
        public string Target { get; }
        public XapkInsertMap(string source, string target)
        {
            Source = source;
            Target = target;
        }
    }
}
