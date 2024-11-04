/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using SharpXapkLib.Exceptions;
using XapkPackagingTool.Common.Data.Model.Xapk;

namespace SharpXapkLib.Utility
{
    internal class MetadataCreator
    {
        internal static string CreateMetadata(XapkManifest xapkManifest)
        {
            try
            {
                return ManifestHandler.SerializeManifest(xapkManifest.DeepClone());
            }
            catch (Exception exc)
            {
                throw new MetadataConvertException(
                    string.Format("MetadataCreateError".Localize(), exc.Message)
                );
            }
        }
    }
}
