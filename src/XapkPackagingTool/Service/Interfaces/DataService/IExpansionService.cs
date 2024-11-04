/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Service.Interfaces.DataService
{
    internal interface IExpansionService
    {
        public List<Expansion> Expansions { get; set; }
    }
}