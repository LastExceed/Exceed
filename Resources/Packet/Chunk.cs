using System.IO;

namespace Resources.Packet {
    public class Chunk {
        public const int packetID = 11;

        public int chunkX;
        public int chunkY;

        public Chunk() {

        }

        public Chunk(BinaryReader reader) {
            chunkX = reader.ReadInt32();
            chunkY = reader.ReadInt32();
        }
    }
}
