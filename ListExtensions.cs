using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIUtils {
    public static class IListExtensions {
        static Random rng = new Random();

        public static IList<T> Shuffle<T>(this IList<T> lst) {
            IList<T> copy = lst.Clone();
            int n = copy.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = copy[k];
                copy[k] = copy[n];
                copy[n] = value;
            }
            return copy;
        }

        public static IList<T> Flatten<T>(this IList<List<T>> lsts) {
            IList<T> flattened = new List<T>();
            foreach(IList<T> lst in lsts) {
                foreach(T val in lst) {
                    flattened.Add(val);
                }
            }
            return flattened;
        }

        public static IList<T> Clone<T>(this IList<T> lst) {
            return lst.Select(i => i).ToList();
        }
    }
}
