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

        public void Send(Player player) {
            //SpinWait.SpinUntil(() => !player.busy);
            //player.busy = true;
            player.writer.Write(packetID);
            player.writer.Write(day);
            player.writer.Write(time);
            //player.busy = false;
        }

        public void Send(Dictionary<ulong, Player> players, ulong toSkip) {
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
