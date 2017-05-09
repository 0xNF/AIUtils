using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIUtils {
    public sealed class Union2<A, B> {
        readonly A Item1;
        readonly B Item2;
        int tag;

        public Union2(A item) { Item1 = item; tag = 0; }
        public Union2(B item) { Item2 = item; tag = 1; }

        public T Match<T>(Func<A, T> f, Func<B, T> g) {
            switch (tag) {
                case 0: return f(Item1);
                case 1: return g(Item2);
                default: throw new Exception("Unrecognized tag value: " + tag);
            }
        }
    }
}
