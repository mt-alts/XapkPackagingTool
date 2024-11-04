/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Common.Utility.ObjectSerialization.Converter
{
    internal class LocalesDictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<Locale>);
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

            var obj = JObject.Load(reader);
            var collection = new List<Locale>();
            foreach (var item in obj)
                collection.Add(
                    new Locale { LanguageCode = item.Key, Name = item.Value.ToString() }
                );
            return collection;
        }

        public override void WriteJson(
            JsonWriter writer,
            object? value,
            Newtonsoft.Json.JsonSerializer serializer
        )
        {
            var collection = value as List<Locale>;
            if (collection != null)
            {
                JObject obj = new JObject();
                foreach (var item in collection)
                    obj.Add(item.LanguageCode, item.Name);
                obj.WriteTo(writer);
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}
