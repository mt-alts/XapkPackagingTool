/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Exceptions
{
    internal class AssetLoadException : Exception
    {
        internal string AssetName { get; private set; }

        internal AssetLoadException(string assetName)
            : base(string.Format("StrAssetLoadFailed".Localize(), assetName))
        {
            AssetName = assetName;
        }

        internal AssetLoadException(string assetName, Exception innerException)
            : base(string.Format("StrAssetLoadFailed".Localize(), assetName), innerException)
        {
            AssetName = assetName;
        }
    }
}
