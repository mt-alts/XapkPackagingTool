/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

namespace XapkPackagingTool.Common.Data.Equality
{
    public interface ICustomEquality<T>
    {
        bool IsEqualTo(T other);
    }
}
