/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.Common.Collection.Generic;
using XapkPackagingTool.Common.Data.Model.Xapk;
using XapkPackagingTool.Service;
using XapkPackagingTool.Service.Interfaces;
using XapkPackagingTool.Service.Interfaces.DataService;
using XapkPackagingTool.Utility.Reader;
using XapkPackagingTool.ViewModel.InputVM;

namespace XapkPackagingTool.ViewModel.Main
{
    internal class ExpansionsViewModel : CollectionViewModel<Expansion>
    {
        private readonly IDialogService _dialogService;
        private readonly IExpansionService _expansionService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IPackageReader _packageReader;

        public ExpansionsViewModel(
            IXapkConfigService xapkConfigService,
            IDialogService dialogService,
            IPackageReader packageReader,
            IMessageDialogService messageDialogService
        )
        {
            _messageDialogService = messageDialogService;
            _dialogService = dialogService;
            _packageReader = packageReader;

            _expansionService = xapkConfigService;
            xapkConfigService.DataChanged += (sender, args) =>
            {
                Items = new SortableBindingList<Expansion>(_expansionService.Expansions);
            };

            Items = new SortableBindingList<Expansion>(_expansionService.Expansions);
        }

        protected override List<Expansion> LoadItemsFromPackage(string path)
        {
            var config = _packageReader.Read(path);
            List<Expansion>? expansions = config.Manifest.Expansions;
            if (expansions == null || expansions.Any())
            {
                _messageDialogService.ShowWarning(
                    "StrNoItemsToImportFound".Localize(),
                    "StrAppName".Localize()
                );
                return new();
            }
            return expansions;
        }

        protected override (bool isResult, object result) ShowDialogForItem(object item = null)
        {
            if (item == null)
                return _dialogService.ShowDialog<ExpansionInputViewModel>();
            else
                return _dialogService.ShowDialog<ExpansionInputViewModel>(item);
        }
    }
}
