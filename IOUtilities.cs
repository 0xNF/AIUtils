using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;


namespace AIUtils {
    public static class IOUtilities {

        public static readonly byte[] utf16_le = new byte[] { 0xFE, 0xFF };
        public static readonly byte[] utf16_be = new byte[] { 0xFF, 0xFE };
        public static readonly byte[] utf8 = new byte[] { 0xEF, 0xBB, 0xBF };
        public static readonly byte[] utf1 = new byte[] { 0xF7, 0x64, 0x4C };
        public static readonly byte[] scsu = new byte[] { 0x0E, 0xFE, 0xFF };
        public static readonly byte[] bocu1 = new byte[] { 0xFB, 0xEE, 0x28 };
        public static readonly byte[] utf32_le = new byte[] { 0x00, 0x00, 0xFE, 0xFF };
        public static readonly byte[] utf32_be = new byte[] { 0xFF, 0xFE, 0x00, 0x00 };
        public static readonly byte[] utf_ebcdic = new byte[] { 0xDD, 0x73, 0x66, 0x73 };
        public static readonly byte[] gb_18030 = new byte[] { 0x84, 0x31, 0x95, 0x33 };
        public static readonly byte[] utf7a = new byte[] { 0x2B, 0x2F, 0x76, 0x38 };
        public static readonly byte[] utf7b = new byte[] { 0x2B, 0x2F, 0x76, 0x39 };
        public static readonly byte[] utf7c = new byte[] { 0x2B, 0x2F, 0x76, 0x2B };
        public static readonly byte[] utf7d = new byte[] { 0x2B, 0x2F, 0x76, 0x2F };
        public static readonly byte[] utf7e = new byte[] { 0x2B, 0x2F, 0x76, 0x38, 0x2D };

        public const byte LINEFEED = 0x0A;
        public const byte CARRIAGE_RETURN = 0x0D;

        public enum EncodingType {
            Unknown = 0,
            UTF_7 = 1,
            UTF_8 = 2,
            UTF_16_LE = 3,
            UTF_16_BE = 4,
            UTF_32_LE = 5,
            UTF_32_BE = 6,
            UTF_EBCDIC = 7,
            GB_18030 = 8,
            BOCU_1 = 9,
            SCSU = 10,
            UTF_1 = 11,
        }

        public static readonly IReadOnlyDictionary<byte[], EncodingType> ByteEncodingMap = new Dictionary<byte[], EncodingType>() {
            {utf_ebcdic, EncodingType.UTF_EBCDIC },
            {scsu, EncodingType.SCSU },
            {bocu1, EncodingType.BOCU_1 },
            {gb_18030, EncodingType.GB_18030 },
            {utf32_be, EncodingType.UTF_32_BE },
            {utf32_le, EncodingType.UTF_32_LE },
            {utf16_be, EncodingType.UTF_16_BE },
            {utf16_le, EncodingType.UTF_16_LE },
            {utf8, EncodingType.UTF_8 },
            {utf1, EncodingType.UTF_1 },
            {utf7a, EncodingType.UTF_7},
            {utf7b, EncodingType.UTF_7},
            {utf7c, EncodingType.UTF_7},
            {utf7d, EncodingType.UTF_7},
            {utf7e, EncodingType.UTF_7},
        };

        public static readonly IReadOnlyDictionary<EncodingType, int> BytesToSkip = new Dictionary<EncodingType, int>() {
            /** 2 byte skip */
            {EncodingType.UTF_16_BE, 2 },
            {EncodingType.UTF_16_LE, 2 },
            /** 3 byte skip */
            {EncodingType.UTF_8, 3 },
            {EncodingType.UTF_1, 3 },
            {EncodingType.SCSU, 3 },
            {EncodingType.BOCU_1, 3 },
            /** 4 byte skip */
            {EncodingType.UTF_32_BE, 4 },
            {EncodingType.UTF_32_LE, 4 },
            {EncodingType.UTF_EBCDIC, 4 },
            {EncodingType.GB_18030, 4 },
            {EncodingType.UTF_7, 4 },
            /** 0 byte skip */
            {EncodingType.Unknown, 0 }            
        };

        public static readonly IReadOnlyDictionary<EncodingType, Encoding> GetEncodingMap = new Dictionary<EncodingType, Encoding>() {
            /** 2 byte skip */
            {EncodingType.UTF_16_BE, Encoding.BigEndianUnicode},
            {EncodingType.UTF_16_LE, Encoding.Unicode},
            /** 3 byte skip */
            {EncodingType.UTF_8, Encoding.UTF8},
            {EncodingType.UTF_1, null},
            {EncodingType.SCSU, Encoding.UTF8},
            {EncodingType.BOCU_1, Encoding.UTF8},
            /** 4 byte skip */
            {EncodingType.UTF_32_BE, null},
            {EncodingType.UTF_32_LE, Encoding.UTF32 },
            {EncodingType.UTF_EBCDIC, null},
            {EncodingType.GB_18030, null},
            {EncodingType.UTF_7, Encoding.UTF7},
            /** 0 byte skip */
            {EncodingType.Unknown, Encoding.UTF8}
        };

        public static EncodingType tryCheckEncoding(byte[] barr) {
            try {
                if (barr.Length > 0) {
                    /** all UTF-7 */
                    if (barr[0] == 0x2B) {
                        if (barr[1] == 0x2F) {
                            if (barr[2] == 0x76) {
                                if (barr[3] == 0x38 || barr[3] == 0x39 || barr[3] == 0x2F || barr[3] == 0x2B) {
                                    return EncodingType.UTF_7;
                                }
                            }
                        }
                    }
                    /** GB */
                    else if (barr[0] == 0x84) {
                        if (barr[1] == 0x31) {
                            if (barr[2] == 0x95) {
                                if (barr[3] == 0x33) {
                                    return EncodingType.GB_18030;
                                }
                            }
                        }
                    }
                    /** EBCDIC */
                    else if (barr[0] == 0xDD) {
                        if (barr[1] == 0x73) {
                            if (barr[2] == 0x66) {
                                if (barr[3] == 0x73) {
                                    return EncodingType.UTF_EBCDIC;
                                }
                            }
                        }
                    }
                    /** Big-Endians */
                    else if (barr[0] == 0xFF) {
                        if(barr[1] == 0xFE) {
                            if(barr[2] == 0x00) {
                                if(barr[3] == 0x00) {
                                    return EncodingType.UTF_32_BE;
                                }
                            }
                            else {
                                return EncodingType.UTF_16_BE;
                            }
                        }
                    }
                    /** UTF-32 Little-Endian */
                    else if(barr[0] == 0x00) {
                        if(barr[1] == 0x00) {
                            if(barr[2] == 0xFE) {
                                if(barr[3] == 0xFF) {
                                    return EncodingType.UTF_32_LE;
                                }
                            }
                        }
                    }
                    /** BOCU-1 */
                    else if (barr[0] == 0xFB) {
                        if(barr[1] == 0xEE) {
                            if(barr[2] == 0x28) {
                                return EncodingType.BOCU_1;
                            }
                        }
                    }
                    /** SCSU */
                    else if (barr[0] == 0x0E) {
                        if(barr[1] == 0xFE) {
                            if(barr[2] == 0xFF) {
                                return EncodingType.SCSU;
                            }
                        }
                    }
                    /** UTF-1 */
                    else if (barr[0] == 0xF7) {
                        if(barr[1] == 0x64) {
                            if(barr[2] == 0x4C) {
                                return EncodingType.UTF_1;
                            }
                        }
                    }
                    /** UTF-8 */
                    else if (barr[0] == 0xEF) {
                        if(barr[1] == 0xBB) {
                            if(barr[2] == 0xBF) {
                                return EncodingType.UTF_8;
                            }
                        }
                    }
                }
                return EncodingType.Unknown;
            }
            catch(Exception e) {
                Console.Error.WriteLine(e); //TODO add logging
                return EncodingType.Unknown;
            }
        }

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


         /// <summary>
        /// Takes an encoding (defaulting to UTF-8) and a function which produces a seekable stream
        /// (or a filename for convenience) and yields lines from the end of the stream backwards.
        /// Only single byte encodings, and UTF-8 and Unicode, are supported. The stream
        /// returned by the function must be seekable.
        /// </summary>
        public sealed class ReverseLineReader : IEnumerable<string> {
            /// <summary>
            /// Buffer size to use by default. Classes with internal access can specify
            /// a different buffer size - this is useful for testing.
            /// </summary>
            private const int DefaultBufferSize = 4096;

            /// <summary>
            /// Means of creating a Stream to read from.
            /// </summary>
            private readonly Func<Stream> streamSource;

            /// <summary>
            /// Encoding to use when converting bytes to text
            /// </summary>
            private readonly Encoding encoding;

            /// <summary>
            /// Size of buffer (in bytes) to read each time we read from the
            /// stream. This must be at least as big as the maximum number of
            /// bytes for a single character.
            /// </summary>
            private readonly int bufferSize;

            /// <summary>
            /// Function which, when given a position within a file and a byte, states whether
            /// or not the byte represents the start of a character.
            /// </summary>
            private Func<long, byte, bool> characterStartDetector;

            /// <summary>
            /// Creates a LineReader from a stream source. The delegate is only
            /// called when the enumerator is fetched. UTF-8 is used to decode
            /// the stream into text.
            /// </summary>
            /// <param name="streamSource">Data source</param>
            public ReverseLineReader(Func<Stream> streamSource)
                : this(streamSource, Encoding.UTF8) {
            }

            /// <summary>
            /// Creates a LineReader from a filename. The file is only opened
            /// (or even checked for existence) when the enumerator is fetched.
            /// UTF8 is used to decode the file into text.
            /// </summary>
            /// <param name="filename">File to read from</param>
            public ReverseLineReader(string filename)
                : this(filename, Encoding.UTF8) {
            }

            /// <summary>
            /// Creates a LineReader from a filename. The file is only opened
            /// (or even checked for existence) when the enumerator is fetched.
            /// </summary>
            /// <param name="filename">File to read from</param>
            /// <param name="encoding">Encoding to use to decode the file into text</param>
            public ReverseLineReader(string filename, Encoding encoding)
                : this(() => File.OpenRead(filename), encoding) {
            }

            /// <summary>
            /// Creates a LineReader from a stream source. The delegate is only
            /// called when the enumerator is fetched.
            /// </summary>
            /// <param name="streamSource">Data source</param>
            /// <param name="encoding">Encoding to use to decode the stream into text</param>
            public ReverseLineReader(Func<Stream> streamSource, Encoding encoding)
                : this(streamSource, encoding, DefaultBufferSize) {
            }

            internal ReverseLineReader(Func<Stream> streamSource, Encoding encoding, int bufferSize) {
                this.streamSource = streamSource;
                this.encoding = encoding;
                this.bufferSize = bufferSize;
                if (encoding.IsSingleByte) {
                    // For a single byte encoding, every byte is the start (and end) of a character
                    characterStartDetector = (pos, data) => true;
                }
                else if (encoding is UnicodeEncoding) {
                    // For UTF-16, even-numbered positions are the start of a character
                    characterStartDetector = (pos, data) => (pos & 1) == 0;
                }
                else if (encoding is UTF8Encoding) {
                    // For UTF-8, bytes with the top bit clear or the second bit set are the start of a character
                    // See http://www.cl.cam.ac.uk/~mgk25/unicode.html
                    characterStartDetector = (pos, data) => (data & 0x80) == 0 || (data & 0x40) != 0;
                }
                else {
                    throw new ArgumentException("Only single byte, UTF-8 and Unicode encodings are permitted");
                }
            }

            /// <summary>
            /// Returns the enumerator reading strings backwards. If this method discovers that
            /// the returned stream is either unreadable or unseekable, a NotSupportedException is thrown.
            /// </summary>
            public IEnumerator<string> GetEnumerator() {
                Stream stream = streamSource();
                if (!stream.CanSeek) {
                    stream.Dispose();
                    throw new NotSupportedException("Unable to seek within stream");
                }
                if (!stream.CanRead) {
                    stream.Dispose();
                    throw new NotSupportedException("Unable to read within stream");
                }
                return GetEnumeratorImpl(stream);
            }

            private IEnumerator<string> GetEnumeratorImpl(Stream stream) {
                try {
                    long position = stream.Length;

                    if (encoding is UnicodeEncoding && (position & 1) != 0) {
                        throw new InvalidDataException("UTF-16 encoding provided, but stream has odd length.");
                    }

                    // Allow up to two bytes for data from the start of the previous
                    // read which didn't quite make it as full characters
                    byte[] buffer = new byte[bufferSize + 2];
                    char[] charBuffer = new char[encoding.GetMaxCharCount(buffer.Length)];
                    int leftOverData = 0;
                    String previousEnd = null;
                    // TextReader doesn't return an empty string if there's line break at the end
                    // of the data. Therefore we don't return an empty string if it's our *first*
                    // return.
                    bool firstYield = true;

                    // A line-feed at the start of the previous buffer means we need to swallow
                    // the carriage-return at the end of this buffer - hence this needs declaring
                    // way up here!
                    bool swallowCarriageReturn = false;

                    while (position > 0) {
                        int bytesToRead = Math.Min(position > int.MaxValue ? bufferSize : (int)position, bufferSize);

                        position -= bytesToRead;
                        stream.Position = position;
                        StreamUtil.ReadExactly(stream, buffer, bytesToRead);
                        // If we haven't read a full buffer, but we had bytes left
                        // over from before, copy them to the end of the buffer
                        if (leftOverData > 0 && bytesToRead != bufferSize) {
                            // Buffer.BlockCopy doesn't document its behaviour with respect
                            // to overlapping data: we *might* just have read 7 bytes instead of
                            // 8, and have two bytes to copy...
                            Array.Copy(buffer, bufferSize, buffer, bytesToRead, leftOverData);
                        }
                        // We've now *effectively* read this much data.
                        bytesToRead += leftOverData;

                        int firstCharPosition = 0;
                        while (!characterStartDetector(position + firstCharPosition, buffer[firstCharPosition])) {
                            firstCharPosition++;
                            // Bad UTF-8 sequences could trigger this. For UTF-8 we should always
                            // see a valid character start in every 3 bytes, and if this is the start of the file
                            // so we've done a short read, we should have the character start
                            // somewhere in the usable buffer.
                            if (firstCharPosition == 3 || firstCharPosition == bytesToRead) {
                                throw new InvalidDataException("Invalid UTF-8 data");
                            }
                        }
                        leftOverData = firstCharPosition;

                        int charsRead = encoding.GetChars(buffer, firstCharPosition, bytesToRead - firstCharPosition, charBuffer, 0);
                        int endExclusive = charsRead;

                        for (int i = charsRead - 1; i >= 0; i--) {
                            char lookingAt = charBuffer[i];
                            if (swallowCarriageReturn) {
                                swallowCarriageReturn = false;
                                if (lookingAt == '\r') {
                                    endExclusive--;
                                    continue;
                                }
                            }
                            // Anything non-line-breaking, just keep looking backwards
                            if (lookingAt != '\n' && lookingAt != '\r') {
                                continue;
                            }
                            // End of CRLF? Swallow the preceding CR
                            if (lookingAt == '\n') {
                                swallowCarriageReturn = true;
                            }
                            int start = i + 1;
                            string bufferContents = new string(charBuffer, start, endExclusive - start);
                            endExclusive = i;
                            string stringToYield = previousEnd == null ? bufferContents : bufferContents + previousEnd;
                            if (!firstYield || stringToYield.Length != 0) {
                                yield return stringToYield;
                            }
                            firstYield = false;
                            previousEnd = null;
                        }

                        previousEnd = endExclusive == 0 ? null : (new string(charBuffer, 0, endExclusive) + previousEnd);

                        // If we didn't decode the start of the array, put it at the end for next time
                        if (leftOverData != 0) {
                            Buffer.BlockCopy(buffer, 0, buffer, bufferSize, leftOverData);
                        }
                    }
                    if (leftOverData != 0) {
                        // At the start of the final buffer, we had the end of another character.
                        throw new InvalidDataException("Invalid UTF-8 data at start of stream");
                    }
                    if (firstYield && string.IsNullOrEmpty(previousEnd)) {
                        yield break;
                    }
                    yield return previousEnd ?? "";
                }
                finally {
                    stream.Dispose();
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }
    }


    // StreamUtil.cs:
    public static class StreamUtil {
        public static void ReadExactly(Stream input, byte[] buffer, int bytesToRead) {
            int index = 0;
            while (index < bytesToRead) {
                int read = input.Read(buffer, index, bytesToRead - index);
                if (read == 0) {
                    throw new EndOfStreamException
                        (String.Format("End of stream reached with {0} byte{1} left to read.",
                                       bytesToRead - index,
                                       bytesToRead - index == 1 ? "s" : ""));
                }
                index += read;
            }
        }
    }
}

