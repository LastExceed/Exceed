using System.IO;

namespace Resources.Packet
{
    public class Sector
    {
        public const int packetID = 12;

        public int sectorX;
        public int sectorY;

        public void read(BinaryReader reader)
        {
            sectorX = reader.ReadInt32();
            sectorY = reader.ReadInt32();
        }
    }
}
