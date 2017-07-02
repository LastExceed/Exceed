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
        UdpClient udp;
        Dictionary<ushort, Player> connections = new Dictionary<ushort, Player>();
        Dictionary<IPEndPoint, ushort> guids = new Dictionary<IPEndPoint, ushort>();
        TcpListener listener;

        Tuple<string, string> encryptionKeys = Hashing.CreateKeyPair();

        public ServerUDP(int port) {
            udp = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            Task.Factory.StartNew(UDPListen);
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Task.Factory.StartNew(TCPListen);
            //loop sending player list to bridge
        }

        public void TCPListen() {
            while(true) {
                Player player = new Player(listener.AcceptTcpClient());
                ushort newGuid = 1;
                while(connections.ContainsKey(newGuid)) {//find lowest available guid
                    newGuid++;
                }
                connections.Add(newGuid, player);

                player.writer.Write(encryptionKeys.Item2);

                var userLength = player.reader.ReadInt32();
                var username = Hashing.Decrypt(encryptionKeys.Item1, player.reader.ReadBytes(userLength));
                
                //get data from db
                var salt = new byte[16];
                var hash = new byte[20];
                player.writer.Write(salt); //send salt

                var hashLength = player.reader.ReadInt32();
                var clientHash = Hashing.DecryptB(encryptionKeys.Item1, player.reader.ReadBytes(hashLength));

                player.writer.Write(0); //No db for now
                /*
                if(hash.SequenceEqual(clientHash)) {
                    player.writer.Write(0); // success;
                } else {
                    player.writer.Write(1); // Wrong password
                }*/
            }
        }
        public void UDPListen() {
            IPEndPoint source = null;
            while(true) {
                byte[] datagram = udp.Receive(ref source);
                //                          Find player from connections where the adreess == source
                ProcessDatagram(datagram, source);
            }
        }

        public void UDPSend(byte[] data, IPEndPoint target) {
            udp.Send(data, data.Length, target);
        }
        public void UDPSend(byte[] data, ushort target) {
            udp.Send(data, data.Length, connections[target].tcp.Client.RemoteEndPoint as IPEndPoint);
        }
        public void UDPbroadcast(byte[] data, IPEndPoint toSkip, bool includeClientless) {
            foreach(var player in connections.Values) {
                var address = player.tcp.Client.RemoteEndPoint as IPEndPoint;
                if(address != toSkip && (player.online || includeClientless)) {
                    udp.Send(data, data.Length, address);
                }
            }
        }

        public void ProcessPacket(int packetID, Player player) {

        }
        public void ProcessDatagram(byte[] datagram, IPEndPoint source) {
            switch((Database.DatagramID)datagram[0]) {
                case Database.DatagramID.entityUpdate:
                    #region entityUpdate
                    break;
                    #endregion
                case Database.DatagramID.attack:
                    #region attack
                    //pass to all players except source
                    break;
                    #endregion
                case Database.DatagramID.shoot:
                    #region shoot
                    //pass to all players except source
                    break;
                    #endregion
                case Database.DatagramID.proc:
                    #region proc
                    //pass to all players except source
                    break;
                    #endregion
                case Database.DatagramID.chat:
                    #region chat
                    //pass to all players
                    break;
                    #endregion
                case Database.DatagramID.interaction:
                    #region interaction
                    //pass to all players except source
                    break;
                    #endregion
                case Database.DatagramID.connect:
                    #region connect
                    Player player = connections[guids[source]];
                    var connect = new Connect(datagram) {
                        Guid = (ushort)player.entityData.guid,
                        Mapseed = 8710 //hardcoded for now
                    };
                    UDPSend(connect.data, source);
                    player.online = true;
                    break;
                    #endregion
                case Database.DatagramID.disconnect:
                    #region disconnect
                    var disconnect = new Disconnect(datagram);
                    connections[disconnect.Guid].online = false;
                    break;
                    #endregion
                default:
                    Console.WriteLine("unknown DatagramID: " + datagram[0]);
                    break;
            }
        }
    }
}
