using System.IO;
using System;

namespace Resources.Utilities {
    public class LongVector {
        public long x, y, z;

        public void Write(BinaryWriter writer) {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }

        public LongVector() { }

        public LongVector(BinaryReader reader) {
            x = reader.ReadInt64();
            y = reader.ReadInt64();
            z = reader.ReadInt64();
        }
    }

    public class IntVector {
        public int x, y, z;

        public void Write(BinaryWriter writer) {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }

        public IntVector() { }
        public IntVector(BinaryReader reader) {
            x = reader.ReadInt32();
            y = reader.ReadInt32();
            z = reader.ReadInt32();
        }
    }

    public class FloatVector {
        public float x, y, z;

        public void Write(BinaryWriter writer) {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }

        public FloatVector() { }
        public FloatVector(BinaryReader reader) {
            x = reader.ReadSingle();
            y = reader.ReadSingle();
            z = reader.ReadSingle();
        }
    }

    public class ByteVector {
        public byte x, y, z;

        public void Write(BinaryWriter writer) {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }

        public ByteVector() { }
        public ByteVector(BinaryReader reader) {
            x = reader.ReadByte();
            y = reader.ReadByte();
            z = reader.ReadByte();
        }
    }
}
