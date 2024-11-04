/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Data.Enums;
using XapkPackagingTool.Constants;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.ViewModel.Main.ApkVariants;

namespace XapkPackagingTool.ViewModel.Main
{
    internal class ApkVariantsViewModel : ViewModelBase
    {
        private readonly IApkVariantService _variantService;

        public bool IsSplitApkModeEnabled
        {
            get => _variantService.VariantSpecies == ApkVariantSpecies.SPLIT;
            set
            {
                _variantService.VariantSpecies = value
                    ? ApkVariantSpecies.SPLIT
                    : ApkVariantSpecies.MONOLITHIC;

                Refresh();

                OnPropertyChanged(nameof(IsSplitApkModeEnabled));
            }
        }

        public IViewModelHost VMHost { get; init; }

        public ApkVariantsViewModel(
            IXapkConfigService xapkConfigService,
            IViewModelHost viewModelHost
        )
        {
            _variantService = xapkConfigService;
            xapkConfigService.DataChanged += (sender, args) =>
            {
                IsSplitApkModeEnabled = _variantService.VariantSpecies == ApkVariantSpecies.SPLIT;
            };

            VMHost = viewModelHost;

            InitializeViewModels();
        }

        private void InitializeViewModels()
        {
            VMHost.AddViewModel<MonolithicApkViewModel>(ApkVariantsViewNavigationKeys.MONOLITHIC);
            VMHost.AddViewModel<SplitsApkViewModel>(ApkVariantsViewNavigationKeys.SPLIT);

            Refresh();
        }

        private void Refresh()
        {
            if (_variantService.VariantSpecies == ApkVariantSpecies.MONOLITHIC)
                VMHost.SwitchViewModelCommand.Execute(ApkVariantsViewNavigationKeys.MONOLITHIC);
            else
                VMHost.SwitchViewModelCommand.Execute(ApkVariantsViewNavigationKeys.SPLIT);
        }
    }
}
