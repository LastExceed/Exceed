using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Resources.Packet {
    public class ProtocolVersion {
        public const int packetID = 17;

        public int version;

        public ProtocolVersion() { }

        public ProtocolVersion(BinaryReader reader) {
            version = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer, bool writePacketID = true) {
            if(writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(version);
        }

        public void Broadcast(Dictionary<ulong, Player> players, long toSkip) {
            foreach(Player player in new List<Player>(players.Values)) {
                if(player.entityData.guid != toSkip) {
                    SpinWait.SpinUntil(() => player.available);
                    player.available = false;
                    try {
                        this.Write(player.writer);
                    } catch { }
                    player.available = true;
                }
            }
        }
    }
}
