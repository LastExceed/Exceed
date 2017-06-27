using System.IO;
using System.Net.Sockets;

namespace Resources {
    public class Player {
        //public bool busy = false;
        public TcpClient tcp;
        public BinaryWriter writer;
        public BinaryReader reader;

        public Packet.EntityUpdate entityData = new Packet.EntityUpdate();

        public Player(TcpClient client) {
            tcp = client;
            writer = new BinaryWriter(tcp.GetStream());
            reader = new BinaryReader(tcp.GetStream());
            entityData.bitfield1 = -1;
            entityData.bitfield2 = -1;
        }
    }
}