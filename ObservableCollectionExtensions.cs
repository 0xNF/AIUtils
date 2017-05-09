using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIUtils {

    public static class ObservableCollectionExtensions {

            public static void reset<T>(this ObservableCollection<T> self, IEnumerable<T> items) {
            self.Clear();
            foreach(T t in items) {
                self.Add(t);
            }
        }

        //public static void forEach<T>(this ObservableCollection<T> self, Action<T> action) {
        //    for(int i = 0; i < self.Count; i++) {
        //        <T>self[i]
        //    }
        //}
    }
}
