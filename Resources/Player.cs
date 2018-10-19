using System.IO;
using System.Net;
using System.Net.Sockets;
using Resources.Packet;

namespace Resources {
    public class Player {
        public TcpClient tcpClient;
        public BinaryWriter writer;
        public BinaryReader reader;
        public EntityUpdate entity;
        public byte Permission;
        public ushort? tomb;
        public string MAC;
        public ushort lastTarget;
        public IPEndPoint RemoteEndPoint {
            get => tcpClient.Client.RemoteEndPoint as IPEndPoint;
        }
        public IPAddress IP {
            get => RemoteEndPoint.Address;
        }

        public Player(TcpClient tcpClient) {
            tcpClient.SendTimeout = 5000;
            //tcpClient.ReceiveTimeout = 60000;
            this.tcpClient = tcpClient;
            var stream = tcpClient.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
    }
}