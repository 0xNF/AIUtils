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


        public static string[] NewLineCharacters = new string[] { "\r\n", "\n" };

    }

}
