using System.IO;

namespace Resources.Packet {
    public class Join {
        public const int packetID = 16;

        public int unknown;
        public long guid;
        public byte[] junk;

        public Join() { }

        public Join(BinaryReader reader) {
            unknown = reader.ReadInt32();
            guid = reader.ReadInt64();
            junk = reader.ReadBytes(0x1168);
        }

        public void Write(BinaryWriter writer, bool writePacketID = true) {
            if(writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(unknown);
            writer.Write(guid);
            writer.Write(junk);
        }
    }
}
