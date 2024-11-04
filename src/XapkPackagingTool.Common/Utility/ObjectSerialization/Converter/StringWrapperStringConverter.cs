/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XapkPackagingTool.Common.Data;

namespace XapkPackagingTool.Common.Utility.ObjectSerialization.Converter
{
    public class StringWrapperStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<StringWrapper>);
        }

        public override object? ReadJson(
            JsonReader reader,
            Type objectType,
            object? existingValue,
            Newtonsoft.Json.JsonSerializer serializer
        )
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var array = JArray.Load(reader);
            var collection = new List<StringWrapper>();
            foreach (var item in array)
                collection.Add(new StringWrapper(item.ToString()));
            return collection;
        }

        public override void WriteJson(
            JsonWriter writer,
            object? value,
            Newtonsoft.Json.JsonSerializer serializer
        )
        {
            var collection = value as List<StringWrapper>;
            if (collection != null)
            {
                JArray array = new JArray();
                foreach (var item in collection)
                    array.Add(item.Content);
                array.WriteTo(writer);
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}
