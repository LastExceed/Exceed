using System.IO;
using System.Threading;

namespace Resources.Packet {
    public class Join {
        public const int packetID = 16;

        public int unknown;
        public ulong guid;
        public byte[] junk;

        public void Read(BinaryReader reader) {
            unknown = reader.ReadInt32();
            guid = reader.ReadUInt64();
            junk = reader.ReadBytes(0x1168);
        }

        public void Write(BinaryWriter writer, bool writePacketID) {
            if (writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(unknown);
            writer.Write(guid);
            writer.Write(junk);
        }
    }
}
