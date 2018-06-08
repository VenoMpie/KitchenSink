using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.WPF.Extensions.Collections
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        public void RemoveRange(int count)
        {
            ActionRange(a => a.RemoveRange(0, count));
        }
        public void RemoveRange(int index, int count)
        {
            ActionRange(a => a.RemoveRange(index, count));
        }
        public void InsertRange(IEnumerable<T> collection)
        {
            ActionRange(a => a.InsertRange(0, collection));
        }
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            ActionRange(a => a.InsertRange(index, collection));
        }
        public void AddRange(IEnumerable<T> collection)
        {
            ActionRange(a => a.AddRange(collection));
        }
        private void ActionRange(Action<List<T>> rangeAction)
        {
            CheckReentrancy();
            var items = Items as List<T>;
            rangeAction(items);
            Reset();
        }

        private void Reset()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
