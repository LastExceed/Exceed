using Resources.Utilities;
using System.IO;

namespace Resources.Packet.Part {
    public class BlockDelta {
        public IntVector position;
        public ByteVector color = new ByteVector();
        public byte type;
        public int unknown;

        public void Read(BinaryReader reader) {
            position.Read(reader);
            color.Read(reader);
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
