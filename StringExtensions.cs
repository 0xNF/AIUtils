using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIUtils {

    public static class StringExtensions {

        public static string BracketedSubstring(this string message, string startString, string stopString) {
            int start = message.IndexOf(startString);
            start += startString.Length;

            int end = message.IndexOf(stopString, start);
            int sublen = end - start;
            string m = message.Substring(start, sublen);
            return m;
        }

        public static string SubstringEndIndex(this string self, int startIndex, int stopIndex) {
            if(startIndex < 0) {
                throw new IndexOutOfRangeException($"startIndex ({startIndex})was less than zero.");
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


        public static string[] NewLineCharacters = new string[] { "\r\n", "\n" };

    }

}
