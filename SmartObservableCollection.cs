using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;


namespace AIUtils {
    public class SmartCollection<T> : ObservableCollection<T> {
        /// <summary>
        /// Stores the items in the list in a way that allows reseting to the original state/sorting without losing original ordering
        /// </summary>
        private List<T> _sourceItems { get; } = new List<T>();
        public int SourceCount {
            get
            {
                return _sourceItems.Count;
            }
        }

        public SmartCollection()
            : base() {
        }

        public SmartCollection(IEnumerable<T> collection)
            : base(collection) {
        }

        public SmartCollection(List<T> list)
            : base(list) {
        }

        public void AddRange(IEnumerable<T> range) {
            foreach (T item in range) {
                Items.Add(item);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        public void ResetWith(IEnumerable<T> range) {
            this._sourceItems.Clear();
            this._sourceItems.AddRange(range);
            this.Items.Clear();
            this.AddRange(range);
        }
        
        public void Reset() {
            this.Items.Clear();
            this.AddRange(this._sourceItems);
        }

        public void SetSourceItems(IEnumerable<T> range) {
            this._sourceItems.Clear();
            this._sourceItems.AddRange(range);
            this.Items.Clear();
            this.AddRange(range);
        }
    }
}
