using System.IO;
using System.Threading;

namespace Resources.Packet {
    public class Join {
        public const int packetID = 16;

        public int unknown;
        public ulong guid;
        public byte[] junk;

        public void Read(BinaryReader reader) {
            unknown = reader.ReadInt32();
            guid = reader.ReadUInt64();
            junk = reader.ReadBytes(0x1168);
        }

        public void Send(Player player) {
            //SpinWait.SpinUntil(() => !player.busy);
            //player.busy = true;
            player.writer.Write(packetID);
            player.writer.Write(unknown);
            player.writer.Write(guid);
            player.writer.Write(junk);
            //player.busy = false;
        }
    }
}
