using System.IO;

namespace Resources.Packet {
    public class Sector {
        public const int packetID = 12;

        public int sectorX;
        public int sectorY;

        public Sector() { }

        public Sector(BinaryReader reader) {
            sectorX = reader.ReadInt32();
            sectorY = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer, bool writePacketID = true) {
            if(writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(sectorX);
            writer.Write(sectorY);
        }
    }
}
