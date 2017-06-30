using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Resources.Packet {
    public class Time {
        public const int packetID = 5;

        public int day;
        public int time;

        public void Read(BinaryReader reader) {
            day = reader.ReadInt32();
            time = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer, bool writePacketID) {
            if (writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(packetID);
            writer.Write(day);
            writer.Write(time);
        }

        public void Broadcast(Dictionary<ulong, Player> players, ulong toSkip) {
            foreach (KeyValuePair<ulong, Player> entry in players) {
                if (entry.Key != toSkip) {
                    //SpinWait.SpinUntil(() => !entry.Value.busy);
                    //entry.Value.busy = true;
                    entry.Value.writer.Write(packetID);
                    entry.Value.writer.Write(day);
                    entry.Value.writer.Write(time);
                    // entry.Value.busy = false;
                }
            }
        }
    }
}
