using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Resources.Packet {
    public class ChatMessage {
        public const int packetID = 10;

        public ulong sender;
        public string message;

        public void read(BinaryReader reader) {
            int length = reader.ReadInt32();
            byte[] mBytes = reader.ReadBytes(length * 2);
            message = Encoding.Unicode.GetString(mBytes);
        }

        public void send(Player player) {
            byte[] mBytes = Encoding.Unicode.GetBytes(message);
            //SpinWait.SpinUntil(() => !player.busy);
            //player.busy = true;
            player.writer.Write(packetID);
            player.writer.Write(sender);
            player.writer.Write(mBytes.Length / 2);
            player.writer.Write(mBytes);
            //player.busy = false;
        }

        public void send(Dictionary<ulong, Player> players, ulong toSkip) {
            byte[] mBytes = Encoding.Unicode.GetBytes(message);
            foreach (Player player in new List<Player>(players.Values)) {
                if (player.entityData.guid != toSkip) {
                    try {
                        player.writer.Write(packetID);
                        player.writer.Write(sender);
                        player.writer.Write(mBytes.Length / 2);
                        player.writer.Write(mBytes);
                    } catch (IOException) {

                    }
                }
            }
        }
    }
}
