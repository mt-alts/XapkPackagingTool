/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Utility.Reflection
{
    public class ReflectionException : Exception
    {
        public ReflectionException() { }

        public ReflectionException(string message)
            : base($"Reflection error: {message}") { }
    }
}
