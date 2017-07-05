using Resources.Utilities;
using System.IO;

namespace Resources.Packet.Part {
    public class BlockDelta {
        public IntVector position;
        public ByteVector color;
        public byte type;
        public int unknown;

        public BlockDelta() { }

        public BlockDelta(BinaryReader reader) {
            position = new IntVector(reader);
            color = new ByteVector(reader);
            type = reader.ReadByte();
            unknown = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer) {
            position.Write(writer);
            color.Write(writer);
            writer.Write(type);
            writer.Write(unknown);
        }
    }
}
