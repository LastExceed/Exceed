using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Resources.Packet
{
    public class MapSeed
    {
        public const int packetID = 15;

        public int seed;

        public void read(BinaryReader reader)
        {
            seed = reader.ReadInt32();
        }

        public void send(Player player)
        {
            //SpinWait.SpinUntil(() => !player.busy);
            //player.busy = true;
            player.writer.Write(packetID);
            player.writer.Write(seed);
            //player.busy = false;
        }
        public void send(Dictionary<ulong, Player> players, ulong toSkip)
        {
            foreach (KeyValuePair<ulong, Player> entry in players)
            {
                if (entry.Key != toSkip)
                {
                    //SpinWait.SpinUntil(() => !entry.Value.busy);
                    //entry.Value.busy = true;
                    entry.Value.writer.Write(packetID);
                    entry.Value.writer.Write(seed);
                    //entry.Value.busy = false;
                }
            }
        }
    }
}
