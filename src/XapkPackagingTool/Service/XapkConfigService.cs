/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.ComponentModel;
using System.Runtime.CompilerServices;
using XapkPackagingTool.Common.Data;
using XapkPackagingTool.Common.Data.Enums;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Common.Data.Model.Xapk.Interfaces;
using XapkPackagingTool.Common.Utility.ObjectSerialization;
using XapkPackagingTool.Common.Utility.Reflection;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Utility.Hashing;

namespace XapkPackagingTool.Service
{
    class XapkConfigService : IXapkConfigService
    {
        private XapkConfig _config;

        public event EventHandler? DataChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public XapkConfig Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged(nameof(Config));
            }
        }

        public IXapkManifest Manifest
        {
            get
            {
                return Config.Manifest;
            }
            set
            {
                ReflectionHelper.TransferProperties(value, Config.Manifest);
                OnPropertyChanged(nameof(Manifest));
            }
        }

        public List<StringWrapper> Permissions
        {
            get
            {
                return Config.Manifest.Permissions ?? new();
            }
            set
            {
                Config.Manifest.Permissions = value;
                OnPropertyChanged(nameof(Manifest));
                OnPropertyChanged(nameof(Permissions));
            }
        }

        public string PackageGlobalName
        {
            get
            {
                return Config.Manifest.Name ?? string.Empty;
            }
            set
            {
                Config.Manifest.PackageName = value;
                OnPropertyChanged(nameof(Manifest));
            }
        }

        public List<Locale> Locales
        {
            get
            {
                return Config.Manifest.Locales ?? new();
            }
            set
            {
                Config.Manifest.Locales = value;
                OnPropertyChanged(nameof(Locales));
            }
        }

        public ApkVariantSpecies VariantSpecies
        {
            get
            {
                return (Config?.Manifest?.XapkVersion <= 1) ?
                    ApkVariantSpecies.MONOLITHIC : ApkVariantSpecies.SPLIT;
            }
            set
            {
                Config.Manifest.XapkVersion = value == ApkVariantSpecies.MONOLITHIC ? 1 : 2;
                OnPropertyChanged(nameof(VariantSpecies));
            }
        }

        public List<SplitApk> SplitApks
        {
            get
            {
                return Config.Manifest.SplitApks ?? new();
            }
            set
            {
                Config.Manifest.SplitApks = value;
                OnPropertyChanged(nameof(SplitApks));
            }
        }

        public string BaseApk
        {
            get
            {
                return Config.BaseApk;
            }
            set
            {
                Config.BaseApk = value;
                OnPropertyChanged(nameof(BaseApk));
            }
        }

        public List<Expansion> Expansions
        {
            get
            {
                return Config.Manifest.Expansions ?? new();
            }
            set
            {
                Config.Manifest.Expansions = value;
                OnPropertyChanged(nameof(Expansions));
            }
        }

        public string BuildPath
        {
            get
            {
                return Config.BuildPath;
            }
            set
            {
                Config.BuildPath = value;
                OnPropertyChanged(nameof(BuildPath));
            }
        }

        public string PackageName => Config
            .Manifest
            .PackageName ?? string.Empty;

        private void InitializeDefaults()
        {
            Config.Manifest ??= new XapkManifest();
            Config.Manifest.Permissions ??= new List<StringWrapper>();
            Config.Manifest.PackageName ??= string.Empty;
            Config.Manifest.Locales ??= new List<Locale>();
            Config.Manifest.SplitApks ??= new List<SplitApk>();
            Config.BaseApk ??= string.Empty;
            Config.Manifest.Expansions ??= new List<Expansion>();
            Config.BuildPath ??= string.Empty;
        }

        public void LoadConfig(XapkConfig config)
        {
            this.Config = config;
            InitializeDefaults();

            OnDataChanged();
        }

        public XapkConfig GetConfig()
        {
            if (Config.Manifest.SplitApks != null && Config.Manifest.SplitApks.Count > 0)
                Config.Manifest.SplitConfigs = Config.Manifest.SplitApks.Select(splitApk => splitApk.Id).ToList();
            return this.Config;
        }

        public string GetConfigHashCode()
        {
            var data = JsonSerializer.SerializeToUtf8Bytes<XapkConfig>(Config);
            return ByteArrayHashCompute.ComputeSha256Hash(data);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnDataChanged()
        {
            DataChanged?.Invoke(this, new EventArgs());
        }
    }
}
