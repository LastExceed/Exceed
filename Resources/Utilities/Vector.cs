using System.IO;

namespace Resources.Utilities {
    public class LongVector {
        public long x;
        public long y;
        public long z;

        public void Read(BinaryReader reader) {
            x = reader.ReadInt64();
            y = reader.ReadInt64();
            z = reader.ReadInt64();
        }

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
        public int x;
        public int y;
        public int z;

        public void Read(BinaryReader reader) {
            x = reader.ReadInt32();
            y = reader.ReadInt32();
            z = reader.ReadInt32();
        }

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
        public float x;
        public float y;
        public float z;

        public void Read(BinaryReader reader) {
            x = reader.ReadSingle();
            y = reader.ReadSingle();
            z = reader.ReadSingle();
        }

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
        public byte x;
        public byte y;
        public byte z;

        public void Read(BinaryReader reader) {
            x = reader.ReadByte();
            y = reader.ReadByte();
            z = reader.ReadByte();
        }

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
