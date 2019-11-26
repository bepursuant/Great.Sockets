using System;
using System.Collections.Generic;
using System.Text;

namespace Great.EmvTags
{
    public static class EmvTagParser
    {
        public static EmvTag ParseOne(byte[] data)
        {

        }

        public static EmvTagList ParseList(byte[] data)
        {

        }

        private static EmvTag Parse(ref byte[] data, ref int offset)
        {
            // `start` and `i` represent cursors along our TLV data. We will move start
            // along until we find the start byte of our tag, length, or value. then
            // we will move i along until we find the end of it. Using their positions
            // we then extract that data, and push them forward one step to look for our
            // next tag, length, or value.

            // rawTlv: PPTTLLLVVVVVVVVPP
            //  start:     ^
            //      i:       ^

            for (int i = offset, start = offset; i < data.Length; start = i)
            {

                // 0x00 can be used as padding before, between, and after tags
                if (IsNullByte(rawTlv[i]))
                {
                    i++;
                    continue;
                }


                // RETRIEVE TAG
                if (IsMultiByteTag(rawTlv[i]))
                {
                    while (!IsLastTagByte(rawTlv[++i])) ;
                }

                int lengthOfTag = (i - start) + 1;
                byte[] tag = new byte[lengthOfTag];
                Array.Copy(rawTlv, start, tag, 0, lengthOfTag);
                start = ++i;


                // RETRIEVE LENGTH
                if (IsMultiByteLength(rawTlv[i]))
                {
                    start++;
                    i += rawTlv[i] - 0x80;
                }

                int lengthOfLength = (i - start) + 1;
                byte[] length = new byte[lengthOfLength];
                Array.Copy(rawTlv, start, length, 0, lengthOfLength);
                start = ++i;


                // RETRIEVE VALUE
                int lengthOfValue = GetInt(length, 0, length.Length);
                byte[] value = new byte[lengthOfValue];
                Array.Copy(rawTlv, start, value, 0, lengthOfValue);
                start = (i += lengthOfValue);


                // build the tag!
                var tlv = new EmvTag(tag, length, value);
                result.Add(tlv);

                // if this was a constructed tag, parse its value into individual Tlv children as well
                if (IsConstructedTag(tag[0]))
                {
                    Parse(tlv.Value, tlv.Children);
                }
            }

            private static bool IsMultiByteLength(byte v) => (v & 0x80) != 0;

            private static bool IsLastTagByte(byte v) => (v & 0x80) == 0;

            private static bool IsMultiByteTag(byte v) => (v & 0x1F) == 0x1F;

            private static bool IsConstructedTag(byte v) => (v & 0x20) != 0;

            private static bool IsNullByte(byte v) => v == 0x00;
        }
    }
}
