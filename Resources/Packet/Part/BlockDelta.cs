using System.IO;

namespace Resources.Packet.Part {
    public class BlockDelta {
        public int posX;
        public int posY;
        public int posZ;
        public byte red;
        public byte green;
        public byte blue;
        public byte type;
        public int unknown;

        public void Read(BinaryReader reader) {
            posX = reader.ReadInt32();
            posY = reader.ReadInt32();
            posZ = reader.ReadInt32();
            red = reader.ReadByte();
            green = reader.ReadByte();
            blue = reader.ReadByte();
            type = reader.ReadByte();
            unknown = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer) {
            writer.Write(posX);
            writer.Write(posY);
            writer.Write(posZ);
            writer.Write(red);
            writer.Write(green);
            writer.Write(blue);
            writer.Write(type);
            writer.Write(unknown);
        }
    }
}
