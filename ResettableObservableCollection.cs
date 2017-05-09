using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIUtils {
    public class ResettableObservableCollection<T> : ObservableCollection<T> {

        private List<T> _source { get; set; } = new List<T>();

        public bool IsEmpty
        {
            get
            {
                return this.Count == 0;
            }
        }

        public ResettableObservableCollection()
            : base() {
        }

        public ResettableObservableCollection(IEnumerable<T> collection)
            : base(collection) {
        }

        public ResettableObservableCollection(List<T> list)
            : base(list) {
        }

        public void AddRange(IEnumerable<T> range) {
            foreach (var item in range) {
                Items.Add(item);
            }
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void StartOverWith(IEnumerable<T> range) {
            this.Items.Clear();
            AddRange(range);
        }

        public void Reset() {
            this.StartOverWith(this._source);
        }

        public void RemoveObservable(T item) {
            this.Items.Remove(item);
        }

        public void Remove(T item) {
            this.RemoveObservable(item);
            this._source.Remove(item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

    }
}
