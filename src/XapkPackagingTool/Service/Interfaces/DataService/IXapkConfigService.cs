/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.ComponentModel;
using XapkPackagingTool.Common.Data.Model.Xapk;

namespace XapkPackagingTool.Service.Interfaces.DataService
{
    interface IXapkConfigService : INotifyPropertyChanged, IManifestService, IPermissionService, ILocaleService, IExpansionService, IApkVariantService, IPackagingOptionsService, IMetadataService
    {
        public event EventHandler? DataChanged;
        public void LoadConfig(XapkConfig config);
        public XapkConfig GetConfig();
        public string GetConfigHashCode();
    }
}
