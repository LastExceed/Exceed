using System;
using System.Text;

namespace Resources {
    public static class Tools {
        public static bool GetBit(this byte value, int bitNumber) {
            return (value & (1 << bitNumber)) != 0;
        }
        public static bool GetBit(this int value, int bitNumber) {
            return (value & (1 << bitNumber)) != 0;
        }
        public static bool GetBit(this long value, int bitNumber) {
            return (value & (1L << bitNumber)) != 0;
        }

        /// <param name="bitnumber">0 based Position</param>
        public static void SetBit(ref byte b, bool value, int bitnumber) {
            if(bitnumber < 8 && bitnumber > -1) {
                if(value) {
                    b |= (byte)(0x01 << bitnumber);
                } else {
                    b &= (byte)~(0x01 << bitnumber);
                }
            } else {
                throw new IndexOutOfRangeException("bitNumber must be between 0-7 for bytes");
            }
        }
        /// <param name="bitNumber">0 based Position</param>
        public static void SetBit(ref int b, bool value, int bitNumber) {
            if(bitNumber < 32 && bitNumber > -1) {
                if(value) {
                    b |= (1 << bitNumber);
                } else {
                    b &= ~(1 << bitNumber);
                }
            } else {
                throw new IndexOutOfRangeException("bitNumber must be between 0-31 for integers");
            }
        }
        /// <param name="bitnumber">0 based Position</param>
        public static void SetBit(ref long b, bool value, int bitnumber) {
            if(bitnumber < 64 && bitnumber > -1) {
                if(value) {
                    b |= 1L << bitnumber;
                } else {
                    b &= ~(1L << bitnumber);
                }
            } else {
                throw new IndexOutOfRangeException("bitNumber must be between 0-63 for bytes");
            }
        }
    }
}