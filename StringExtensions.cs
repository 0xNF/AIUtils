using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIUtils {

    public static class StringExtensions {

        public static string BracketedSubstring(this string self, string startString, string stopString) {
            int start = self.IndexOf(startString);
            start += startString.Length;

            int end = self.IndexOf(stopString, start);
            int sublen = end - start;
            string m = self.Substring(start, sublen);
            return m;
        }

        public static string SubstringEndIndex(this string self, int startIndex, int stopIndex) {
            if(startIndex < 0) {
                throw new IndexOutOfRangeException($"startIndex ({startIndex}) was less than zero.");
            }
            if(startIndex >= self.Length) {
                throw new IndexOutOfRangeException($"startIndex ({startIndex}) was greater than length of the string ({self.Length})");
            }
            if (stopIndex >= self.Length) {
                throw new IndexOutOfRangeException($"stopIndex ({stopIndex}) was greater than or equal to the length of the string ({self.Length})");
            }
            if(stopIndex < startIndex) {
                throw new IndexOutOfRangeException($"stopIndex ({stopIndex}) was less than or equal to the length of the startIndex ({startIndex})");
            }
            if(self.Length == 0 || string.IsNullOrEmpty(self)) {
                return self;
            }
            if(startIndex == stopIndex) {
                return string.Empty;
            }
            return self.Substring(startIndex, stopIndex-startIndex);
        }

        public static int NthIndexOf(this string input,
                                     string value, int startIndex, int nth) {
            if (nth < 1)
                throw new NotSupportedException("Param 'nth' must be greater than 0!");
            if (nth == 1)
                return input.IndexOf(value, startIndex);
            var idx = input.IndexOf(value, startIndex);
            if (idx == -1)
                return -1;
            return input.NthIndexOf(value, idx + 1, --nth);
        }

        public static int[] AllIndexesOf(this string self, char value) {
            List<int> ints = new List<int>();
            for(int i = 0;  i < self.Length; i++) {
                if(self[i] == value) {
                    ints.Add(i);
                }
            }
            return ints.ToArray();
        }

        public static string[] NewLineCharacters = new string[] { "\r\n", "\n" };

    }

}
