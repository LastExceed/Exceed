using System;

namespace Resources {
    public static class Tools {
        public static bool GetBit(this byte value, int bitNumber) {
            return (value & (1 << bitNumber)) != 0;
        }

        public static bool GetBit(this int value, int bitNumber) {
            return (value & (1 << bitNumber)) != 0;
        }
        
        /// <param name="position">0 based Position</param>
        public static void SetBit(this byte b, bool value, int position) {
            if(position < 8 && position > -1) {
                if(value) {
                    b |= (byte)(0x01 << position);
                } else {
                    b &= (byte)~(0x01 << position);
                }
            } else {
                throw new InvalidOperationException("Die position ist außerhalb des byte bereichs");
            }
        }

        /// <param name="position">0 based Position</param>
        public static void SetBit(this int b, bool value, int position) {
            if(position < 8 && position > -1) {
                if(value) {
                    b |= (byte)(0x01 << position);
                } else {
                    b &= (byte)~(0x01 << position);
                }
            } else {
                throw new InvalidOperationException("Die position ist außerhalb des byte bereichs");
            }
        }
    }
}