using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Resources {
    public class Player {
        //public bool busy = false;
        public TcpClient tcp;
        public BinaryWriter writer;
        public BinaryReader reader;
        public bool playing = false;
        public IPEndPoint Address { get; private set; }
        public Packet.EntityUpdate entityData = new Packet.EntityUpdate();
        public string username;

        public Player(TcpClient client) {
            tcp = client;
            tcp.NoDelay = true;
            writer = new BinaryWriter(tcp.GetStream());
            reader = new BinaryReader(tcp.GetStream());
            Address = tcp.Client.RemoteEndPoint as IPEndPoint;
        }
    }
}