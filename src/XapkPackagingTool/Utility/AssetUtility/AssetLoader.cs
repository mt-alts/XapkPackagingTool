/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.IO;
using XapkPackagingTool.Common.Utility.ObjectSerialization;
using XapkPackagingTool.Exceptions;

namespace XapkPackagingTool.Utility.AssetUtility
{
    internal class AssetLoader<TObj>
    {
        private static readonly object LockObject = new object();
        private static Dictionary<string, TObj> dataCache = new Dictionary<string, TObj>();

        public static TObj LoadData(string filePath)
        {
            try
            {
                if (dataCache.ContainsKey(filePath))
                    return dataCache[filePath];

                if (!File.Exists(filePath))
                    throw new FileNotFoundException(
                        string.Format("StrFileNotFoundMessage".Localize(), filePath)
                    );

                lock (LockObject)
                {
                    if (!dataCache.ContainsKey(filePath))
                    {
                        TObj data = JsonSerializer.Deserialize<TObj>(File.ReadAllText(filePath));
                        if (data == null)
                            throw new AssetLoadException(filePath);
                        dataCache.Add(filePath, data);
                    }

                    return dataCache[filePath];
                }
            }
            catch (Exception)
            {
                throw new Exceptions.AssetLoadException(filePath);
            }
        }
    }
}
