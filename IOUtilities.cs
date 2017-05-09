using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIUtils {
    public static class IOUtilities {
        /// <summary>
        /// Attempts to determine the Unicode Encoding of the file
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static System.Text.UnicodeEncoding TryDetectUnicodeEncoding(System.IO.Stream s) {
            int byteCount = 5;
            byte[] bomBuffer = new byte[byteCount]; //Up to 4 bytes
            int bytesRead = s.Read(bomBuffer, 0, byteCount);

            byte[] utf16BE = { 254, 255 };
            byte[] utf16LE = { 255, 254 };
            byte[] utf8 = { 239, 187, 191 };
            byte[] utf32BE = { 0, 0, 254, 255 };
            byte[] utf32LE = { 255, 254, 0, 0 };
            byte[][] utf7s = {
                new byte[] { 43, 47, 118, 56, 45 },
                new byte[]  {43, 47, 118, 56 },
                new byte[] { 43, 47, 118, 57 },
                new byte[] { 43, 47, 118, 43 },
                new byte[] { 43, 47, 118, 47 },
        };

            System.Text.UnicodeEncoding e = Encoding.ASCII as System.Text.UnicodeEncoding;

            if (bytesRead < 5) {
                throw new Exception("File has no data");
            }

            if (bomBuffer.Take(utf32LE.Length).SequenceEqual(utf32LE)) {
                e = Encoding.UTF32 as System.Text.UnicodeEncoding;
            }
            else if (bomBuffer.Take(utf32BE.Length).SequenceEqual(utf32BE)) {
                //Wat do
            }
            else if (bomBuffer.Take(utf8.Length).SequenceEqual(utf8)) {
                e = System.Text.Encoding.UTF8 as System.Text.UnicodeEncoding;
            }
            else if (bomBuffer.Take(utf16LE.Length).SequenceEqual(utf16LE)) {
                e = System.Text.Encoding.Unicode as System.Text.UnicodeEncoding;
            }
            else if (bomBuffer.Take(utf16BE.Length).SequenceEqual(utf16BE)) {
                e = System.Text.Encoding.BigEndianUnicode as System.Text.UnicodeEncoding;
            }
            else {
                foreach (byte[] barr in utf7s) {
                    if (bomBuffer.Take(barr.Length).SequenceEqual(barr)) {
                        e = System.Text.Encoding.UTF7 as System.Text.UnicodeEncoding;
                    }
                }
            }
            s.Seek(0, System.IO.SeekOrigin.Begin);
            return e;
        }

        /// <summary>
        /// Returns the UnicodeEncoding as a Storage Enumeration value
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Windows.Storage.Streams.UnicodeEncoding GetUniodeEncoding(System.IO.Stream s) {
            UnicodeEncoding e = TryDetectUnicodeEncoding(s);
            if(e == Encoding.Unicode) {
                return Windows.Storage.Streams.UnicodeEncoding.Utf16LE;
            }else if(e == Encoding.BigEndianUnicode) {
                return Windows.Storage.Streams.UnicodeEncoding.Utf16BE;
            }
            else if(e == Encoding.UTF8) {
                return Windows.Storage.Streams.UnicodeEncoding.Utf8;
            }
            else {
                throw new Exception("Could not detect encoding. Is file unicode?");
            }
        }
    }
}
