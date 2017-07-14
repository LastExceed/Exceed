using System.IO;

namespace Resources.Utilities {
    public class LongVector {
        public long x, y, z;

        public LongVector() { }
        public LongVector(BinaryReader reader) {
            x = reader.ReadInt64();
            y = reader.ReadInt64();
            z = reader.ReadInt64();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }
    }

    public class IntVector {
        public int x, y, z;

        public IntVector() { }
        public IntVector(BinaryReader reader) {
            x = reader.ReadInt32();
            y = reader.ReadInt32();
            z = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }
    }

    public class FloatVector {
        public float x, y, z;

        public FloatVector() { }
        public FloatVector(BinaryReader reader) {
            x = reader.ReadSingle();
            y = reader.ReadSingle();
            z = reader.ReadSingle();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }
    }

    public class ByteVector {
        public byte x, y, z;

        public ByteVector() { }
        public ByteVector(BinaryReader reader) {
            x = reader.ReadByte();
            y = reader.ReadByte();
            z = reader.ReadByte();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }
    }
}
