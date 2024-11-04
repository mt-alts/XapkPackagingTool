/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/
using Newtonsoft.Json;

namespace XapkPackagingTool.Common.Utility.ObjectSerialization
{
    public static class JsonSerializer
    {
        public static string Serialize<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static byte[] SerializeToUtf8Bytes<T>(T obj)
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(obj);
        }
    }
}
