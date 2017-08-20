using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Resources.Packet;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Resources {
    public class Player {
        public TcpClient tcp;
        //public Stream stream;
        public BinaryWriter writer;
        public BinaryReader reader;
        public bool playing = false;
        public bool available = true;
        public IPEndPoint Address { get; private set; }
        public EntityUpdate entityData = new EntityUpdate();
        public string username;
        public Stopwatch lagMeter;

        public Player(TcpClient client) : this(client, null) { }

        public Player(TcpClient client, X509Certificate serverCert) {
            tcp = client;
            tcp.NoDelay = true;

            Stream stream;
            if(serverCert == null) {
                stream = tcp.GetStream();
            } else {
                var s = new SslStream(tcp.GetStream());
                s.AuthenticateAsServer(serverCert);
                stream = s;
            }

            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);

            Address = tcp.Client.RemoteEndPoint as IPEndPoint;
            lagMeter = new Stopwatch();
            lagMeter.Start();
        }
    }
}