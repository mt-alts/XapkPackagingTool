/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data;

namespace XapkPackagingTool.Service.Interfaces.DataService
{
    interface IPermissionService
    {
        public List<StringWrapper> Permissions { get; set; }
    }
}
