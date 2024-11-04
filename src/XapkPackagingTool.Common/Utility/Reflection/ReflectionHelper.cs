/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Reflection;

namespace XapkPackagingTool.Common.Utility.Reflection
{
    public static class ReflectionHelper
    {
        public static object TransferProperties(object source, object destination)
        {
            try
            {
                var sourceType = source.GetType();
                var destinationType = destination.GetType();

                foreach (var prop in sourceType.GetProperties())
                {
                    if (prop.GetCustomAttribute<SkipTransferAttribute>() != null)
                        continue;

                    var destinationProp = destinationType.GetProperty(prop.Name);

                    if (destinationProp != null && destinationProp.CanWrite)
                    {
                        var value = prop.GetValue(source);
                        destinationProp.SetValue(destination, value);
                    }
                }
                return destination;
            }
            catch (Exception exc)
            {
                throw new ReflectionException(exc.Message);
            }
        }
    }
}
