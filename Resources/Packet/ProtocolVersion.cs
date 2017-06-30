using System.Collections.Generic;
using System.IO;

namespace Resources.Packet {
    public class ProtocolVersion {
        public const int packetID = 17;

        public int version;

        public ProtocolVersion() {

        }

        public ProtocolVersion(BinaryReader reader) {
            version = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer, bool writePacketID) {
            if (writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(packetID);
            writer.Write(version);
        }

        public void Broadcast(Dictionary<ulong, Player> players, ulong toSkip) {
            foreach (KeyValuePair<ulong, Player> entry in players) {
                if (entry.Key != toSkip) {
                    //SpinWait.SpinUntil(() => !entry.Value.busy);
                    //entry.Value.busy = true;
                    entry.Value.writer.Write(packetID);
                    entry.Value.writer.Write(version);
                    //entry.Value.busy = false;
                }
            }
        }
    }
}
