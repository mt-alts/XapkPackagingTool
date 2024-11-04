/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XapkPackagingTool.Common.Collection.Generic
{
    public static class BooleanValuePair
    {
        public static BooleanValuePair<TValue> Create<TValue>(bool boolean, TValue value) =>
            new BooleanValuePair<TValue>(boolean, value);

        internal static string PairToString(object? boolean, object? value) =>
            $"[{boolean}, {value}]";
    }

    [Serializable]
    public class BooleanValuePair<TValue> : INotifyPropertyChanged
    {
        private bool boolean;
        private TValue tvalue;

        public event PropertyChangedEventHandler? PropertyChanged;

        public BooleanValuePair(bool boolean, TValue value)
        {
            this.boolean = boolean;
            this.tvalue = value;
        }

        public BooleanValuePair() { }

        public bool Bool
        {
            get { return boolean; }
            set
            {
                boolean = value;
                OnPropertyChanged(nameof(Bool));
            }
        }

        public TValue Value
        {
            get { return tvalue; }
            set
            {
                tvalue = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public override string ToString()
        {
            return BooleanValuePair.PairToString(Bool, Value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out bool boolean, out TValue value)
        {
            boolean = Bool;
            value = Value;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
