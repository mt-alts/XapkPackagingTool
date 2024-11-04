/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Service.Interfaces
{
    internal interface IModelService
    {
        public XapkConfig Model { get; }

        public void LoadModel(XapkConfig config);
    }
}
