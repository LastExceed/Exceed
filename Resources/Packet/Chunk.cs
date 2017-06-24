using System.IO;

namespace Resources.Packet
{
    public class Chunk
    {
        public const int packetID = 11;

        public int chunkX;
        public int chunkY;

        public void read(BinaryReader reader)
        {
            chunkX = reader.ReadInt32();
            chunkY = reader.ReadInt32();
        }
    }
}
