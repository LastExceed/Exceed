using System.IO;

namespace Resources.Packet {
    public class Sector {
        public const int packetID = 12;

        public int sectorX;
        public int sectorY;

        public Sector() {

        }

        public Sector(BinaryReader reader) {
            sectorX = reader.ReadInt32();
            sectorY = reader.ReadInt32();
        }
    }
}
