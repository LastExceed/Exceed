using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Resources;
using Resources.Datagram;
using System.IO;
using System.Threading;

namespace Server {
    class ServerUDP {
        public class Player {
            public TcpClient tcp;
            public BinaryWriter writer;
            public BinaryReader reader;
            public string name;
            public ushort guid;

            public Player(TcpClient client, ServerUDP server) {
                tcp = client;
                writer = new BinaryWriter(tcp.GetStream());
                reader = new BinaryReader(tcp.GetStream());
                Task.Factory.StartNew(() => Listen(server));
            }

            public void Listen(ServerUDP server) {
                int packetID = -1;
                while(tcp.Connected) {
                    try {
                        packetID = reader.ReadInt32();
                    } catch(IOException) {
                        server.Kick(this);
                        break;
                    }
                    server.ProcessTCP( packetID, this);
                }
            }
        }
        
        UdpClient UDPclient;
        List<Player> clients = new List<Player>();
        TcpListener TCPserver;

        public ServerUDP(int port) {
            UDPclient = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            Task.Factory.StartNew(UDPListen);
            TCPserver = new TcpListener(IPAddress.Any, port);
            TCPserver.Start();
            Task.Factory.StartNew(TCPListen);
        }

        public void TCPListen() {
            while(true) {
                clients.Add(new Player(TCPserver.AcceptTcpClient(),this));
            }
        }
        public void UDPListen() {
            IPEndPoint source = null;
            while(true) {
                byte[] data = UDPclient.Receive(ref source);
                ProcessPacket(data, source);
            }
        }

        public void UDPSendToAll(byte[] data, IPEndPoint source = null) {
            foreach(var item in clients) {
                var adress = item.tcp.Client.RemoteEndPoint as IPEndPoint;
                if(adress != source)
                    UDPclient.Send(data, data.Length, adress);
            }
        }

        public void Alert(string message, ConsoleColor color = ConsoleColor.White) {
             var p = new Chat(message) {
                Sender = 0
            };
            UDPSendToAll(p.data);
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
        
        public void Kick(Player player) {
            clients.Remove(player);
            player.tcp.Close();
            Alert(player.name + "left", ConsoleColor.DarkRed);

            Disconnect p = new Disconnect() {
                Guid = player.guid
            };

            UDPSendToAll(p.data);
        }

        public void ProcessTCP(int packetID, Player player) {
            throw new NotImplementedException();
        }
        public void ProcessPacket(byte[] packet, IPEndPoint source) {
            throw new NotImplementedException();

            var type = (Database.DatagramID)packet[0];
            switch(type) {
                case Database.DatagramID.entityUpdate:
                    break;
                case Database.DatagramID.hit:
                    break;
                case Database.DatagramID.shoot:
                    break;
                case Database.DatagramID.proc:
                    break;
                case Database.DatagramID.chat:
                    break;
                case Database.DatagramID.time:
                    break;
                case Database.DatagramID.interaction:
                    break;
                case Database.DatagramID.staticUpdate:
                    break;
                case Database.DatagramID.block:
                    break;
                case Database.DatagramID.particle:
                    break;
                case Database.DatagramID.connect:
                    break;
                case Database.DatagramID.disconnect:
                    break;
                case Database.DatagramID.players:
                    break;
                default:
                    Console.WriteLine("unknown packet ID: " + type); //causes some console spam, but allows resyncing with the player without DC or crash
                    break;
            }
        }
    }
}
