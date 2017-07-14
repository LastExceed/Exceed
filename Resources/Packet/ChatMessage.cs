using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Resources.Packet {
    public class ChatMessage {
        public const int packetID = 10;

        public ulong sender;
        public string message;

        public ChatMessage() { }

        public ChatMessage(BinaryReader reader) {
            int length = reader.ReadInt32();
            byte[] mBytes = reader.ReadBytes(length * 2);
            message = Encoding.Unicode.GetString(mBytes);
        }

        public void Write(BinaryWriter writer, bool writePacketID = true) {
            byte[] mBytes = Encoding.Unicode.GetBytes(message);

            if(writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(sender);
            writer.Write(mBytes.Length / 2);
            writer.Write(mBytes);
        }

        public void Broadcast(Dictionary<ulong, Player> players, ulong toSkip) {
            byte[] mBytes = Encoding.Unicode.GetBytes(message);
            foreach(Player player in new List<Player>(players.Values)) {
                if(player.entityData.guid != toSkip) {
                    SpinWait.SpinUntil(() => player.available);
                    player.available = false;
                    try
                    {
                        player.writer.Write(packetID);
                        player.writer.Write(sender);
                        player.writer.Write(mBytes.Length / 2);
                        player.writer.Write(mBytes);
                    } catch { }
                    player.available = true;
                }
            }
        }
    }
}
