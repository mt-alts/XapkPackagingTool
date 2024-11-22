/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.ComponentModel;
using XapkPackagingTool.Common.Data.Equality;

namespace XapkPackagingTool.Common.Data
{
    public class StringWrapper : INotifyPropertyChanged, ICloneable, ICustomEquality<StringWrapper>
    {
        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public StringWrapper(string value)
        {
            Content = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsEqualTo(StringWrapper other)
        {
            if (other != null)
                return Content.Equals(other.Content);
            return false;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
