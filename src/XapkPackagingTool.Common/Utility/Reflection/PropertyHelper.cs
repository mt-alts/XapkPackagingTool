/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Reflection;

namespace XapkPackagingTool.Common.Utility.Reflection
{
    public class PropertyHelper
    {
        public static void SetPropertyValue<T>(T model, string propertyName, object value)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Type type = typeof(T);
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            if (propertyInfo == null)
                throw new ArgumentException(
                    $"Property {propertyName} not found in type {type.Name}"
                );

            if (!propertyInfo.CanWrite)
                throw new ArgumentException($"Property {propertyName} is read-only");

            try
            {
                propertyInfo.SetValue(model, Convert.ChangeType(value, propertyInfo.PropertyType));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"Error setting property {propertyName}: {ex.Message}",
                    ex
                );
            }
        }
    }
}
