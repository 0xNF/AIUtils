using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIUtils {
    public static class IEnumerableExtensions {

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> list) {
            List<T> flat = new List<T>();
            foreach(IEnumerable<T> sublist in list) {
                flat.AddRange(sublist);
            }
            return flat;
        }
    }
}
