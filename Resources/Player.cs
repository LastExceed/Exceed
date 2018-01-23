using System.IO;
using System.Net.Sockets;
using Resources.Packet;

namespace Resources {
    public class Player {
        public TcpClient tcpClient;
        public BinaryWriter writer;
        public BinaryReader reader;
        public EntityUpdate entity;
        public ushort? tomb;
        public string MAC;
        public ushort lastTarget;

        public Player(TcpClient tcpClient) {
            this.tcpClient = tcpClient;
            var stream = tcpClient.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
    }
}