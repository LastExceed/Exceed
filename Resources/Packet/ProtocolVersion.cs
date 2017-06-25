using System.Collections.Generic;
using System.IO;

namespace Resources.Packet {
    public class ProtocolVersion {
        public const int packetID = 17;

        public int version;

        public void Read(BinaryReader reader) {
            version = reader.ReadInt32();
        }

        public void Send(Player player) {
            //SpinWait.SpinUntil(() => !player.busy);
            //player.busy = true;
            player.writer.Write(packetID);
            player.writer.Write(version);
            //player.busy = false;
        }

        public void Send(Dictionary<ulong, Player> players, ulong toSkip) {
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
