/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using XapkPackagingTool.ViewModel.Interfaces;

namespace XapkPackagingTool.ViewModel.InputVM
{
    public abstract class InputViewModelBase : ViewModelBase, IResultable
    {
        public bool IsRequestClose { get; protected set; } = false;

        public virtual object Result { get; } = null!;

    }
}