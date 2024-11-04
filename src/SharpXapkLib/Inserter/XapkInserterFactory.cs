/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpXapkLib.Utility;

namespace SharpXapkLib.Inserter
{
    internal class XapkInserterFactory : IXapkInserterFactory
    {
        public XapkInserter CreateInserter(XapkFileMap xapkFileMap, string outputXapkFile)
           => new XapkInserter(xapkFileMap, outputXapkFile);
    }
}
