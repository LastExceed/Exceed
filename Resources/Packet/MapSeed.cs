using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Resources.Packet {
    public class MapSeed {
        public const int packetID = 15;

        public int seed;

        public MapSeed() { }

        public MapSeed(BinaryReader reader) {
            seed = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer, bool writePacketID = true) {
            if(writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(seed);
        }
        public void Broadcast(Dictionary<ulong, Player> players, ulong toSkip) {
            foreach(KeyValuePair<ulong, Player> entry in players) {
                if(entry.Key != toSkip) {
                    Write(entry.Value.writer);
                }
            }
        }
    }
}
