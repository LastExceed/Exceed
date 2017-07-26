using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Resources.Packet;

namespace Resources {
    public class Player {
        public TcpClient tcp;
        public BinaryWriter writer;
        public BinaryReader reader;
        public bool playing = false;
        public bool available = true;
        public IPEndPoint Address { get; private set; }
        public EntityUpdate entityData = new EntityUpdate();
        public string username;
        public Stopwatch lagMeter;

        public Player(TcpClient client) {
            tcp = client;
            tcp.NoDelay = true;
            writer = new BinaryWriter(tcp.GetStream());
            reader = new BinaryReader(tcp.GetStream());
            Address = tcp.Client.RemoteEndPoint as IPEndPoint;
            lagMeter = new Stopwatch();
            lagMeter.Start();
        }
    }
}