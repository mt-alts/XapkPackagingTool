/*
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.ComponentModel;

namespace XapkPackagingTool.Common.Collection.Generic
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool isSorted;
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;

        protected override bool SupportsSortingCore => true;
        protected override bool IsSortedCore => isSorted;
        protected override PropertyDescriptor SortPropertyCore => sortProperty;
        protected override ListSortDirection SortDirectionCore => sortDirection;

        public SortableBindingList() { }

        public SortableBindingList(IList<T> list)
            : base(list) { }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            var items = (List<T>)Items;
            items.Sort(new PropertyComparer<T>(prop, direction));
            sortProperty = prop;
            sortDirection = direction;
            isSorted = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
            sortProperty = null;
        }

        private class PropertyComparer<U> : IComparer<U>
        {
            private readonly PropertyDescriptor prop;
            private readonly ListSortDirection direction;

            public PropertyComparer(PropertyDescriptor prop, ListSortDirection direction)
            {
                this.prop = prop;
                this.direction = direction;
            }

            public int Compare(U x, U y)
            {
                var xValue = prop.GetValue(x);
                var yValue = prop.GetValue(y);
                return direction == ListSortDirection.Ascending
                    ? Comparer<object>.Default.Compare(xValue, yValue)
                    : Comparer<object>.Default.Compare(yValue, xValue);
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            RaiseListChangedEvents = false;

            foreach (var item in items)
                Items.Add(item);

            RaiseListChangedEvents = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
    }
}
