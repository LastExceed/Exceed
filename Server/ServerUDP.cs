using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Resources;
using Resources.Datagram;
using System.Text.RegularExpressions;

namespace Server {
    class ServerUDP {
        UdpClient udp;
        TcpListener listener;
        Dictionary<ushort, Player> connections = new Dictionary<ushort, Player>();
        Dictionary<IPEndPoint, ushort> guids = new Dictionary<IPEndPoint, ushort>();
        Dictionary<ushort, IPEndPoint> reverseGuids = new Dictionary<ushort, IPEndPoint>();

        Tuple<string, string> encryptionKeys = Hashing.CreateKeyPair();

        public ServerUDP(int port) {
            udp = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            Task.Factory.StartNew(ListenUDP);
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Task.Factory.StartNew(ListenTCP);
        }

        public void ListenTCP() {
            while(true) {
                Player player = new Player(listener.AcceptTcpClient());
                ushort newGuid = 1;
                while(connections.ContainsKey(newGuid)) {//find lowest available guid
                    newGuid++;
                }
                connections.Add(newGuid, player);

                #region secure login
                player.writer.Write(encryptionKeys.Item2); // Send rsa encryption key
                
                var username = Hashing.Decrypt(encryptionKeys.Item1, player.reader.ReadBytes(player.reader.ReadInt32())); // Read username

                testEntities db = new testEntities();
                var row = (from o in db.users where o.username == username select o).FirstOrDefault(); // Get user with username from db

                if(row != null) { // if row is not empty
                    var hashData = row.hash.Split('$'); // Split hash from db
                    var hashBytes = Convert.FromBase64String(hashData[4]); //Get hasbytes = salt + hash

                    var salt = new byte[16];
                    Array.Copy(hashBytes, 0, salt, 0, 16); // extract salt
                    var hash = new byte[20];
                    Array.Copy(hashBytes, 16, hash, 0, 20);// extract hash

                    player.writer.Write(salt); //send salt
                    
                    var clientHash = Hashing.DecryptB(encryptionKeys.Item1, player.reader.ReadBytes(player.reader.ReadInt32())); // Get clientside hashed password

                    if(hash.SequenceEqual(clientHash)) {
                        player.writer.Write(0); // success;
                        continue;
                    }
                } else {
                    // Advance with random data so bridge dosen't get stuck
                    var bytes = new byte[16];
                    new Random().NextBytes(bytes);
                    player.writer.Write(bytes);
                    player.reader.ReadBytes(player.reader.ReadInt32());
                }

                player.writer.Write(1); // login failed
                #endregion
            }
        }
        public void ListenUDP() {
            IPEndPoint source = null;
            while(true) {
                byte[] datagram = udp.Receive(ref source);
                if(!guids.ContainsKey(source)) {
                    var guid = connections.First(x => (x.Value.tcp.Client.RemoteEndPoint as IPEndPoint).Address.ToString() == source.Address.ToString()).Key;
                    guids.Add(source, guid);
                    reverseGuids.Add(guid, source);
                }
                ProcessDatagram(datagram, source);
            }
        }

        public void SendUDP(byte[] data, IPEndPoint target) {
            udp.Send(data, data.Length, target);
        }
        public void SendUDP(byte[] data, ushort target) {
            udp.Send(data, data.Length, reverseGuids[target]);
        }
        public void BroadcastUDP(byte[] data, IPEndPoint toSkip = null, bool includeNotPlaying = false) {
            foreach(var item in guids) {
                if(!item.Key.Equals(toSkip) && (connections[item.Value].playing || includeNotPlaying)) {
                    SendUDP(data, item.Key);
                }
            }
        }

        public void ProcessPacket(int packetID, Player player) {

        }
        public void ProcessDatagram(byte[] datagram, IPEndPoint source) {
            switch((Database.DatagramID)datagram[0]) {
                case Database.DatagramID.entityUpdate:
                    #region entityUpdate
                    BroadcastUDP(datagram, source);
                    break;
                #endregion
                case Database.DatagramID.attack:
                    #region attack
                    BroadcastUDP(datagram, source); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.shoot:
                    #region shoot
                    BroadcastUDP(datagram, source); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.proc:
                    #region proc
                    BroadcastUDP(datagram, source); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.chat:
                    #region chat
                    var chat = new Chat(datagram);
                    if(chat.Text.StartsWith("/")) {
                        var match = Regex.Match(chat.Text, @"(?P<command>(?<=\/)\w+) (?P<parameter>.+)");
                        var command = match.Groups["command"].Value;
                        var parameter = match.Groups["parameter"].Value;
                        switch(match.Groups["command"].Value) {
                            case "spawn":
                                break;

                            case "reload_world":
                                break;

                            case "xp":
                                /*
                                try {
                                    int amount = Convert.ToInt32(parameter);
                                    
                                    var xpDummy = new EntityUpdate();
                                    xpDummy.shit.guid = 1000;
                                    //xpDummy.shit.Bitfield = 0b00000000_00000000_00000000_10000000;
                                    xpDummy.shit.hostility = (byte)Database.Hostility.enemy;

                                    UDPbroadcast(xpDummy.getData());

                                    var kill = new Kill() {
                                        killer = connections[guids[source]].entityData.guid,
                                        victim = 1000,
                                        xp = amount
                                    };
                                    var serverUpdate = new ServerUpdate();
                                    serverUpdate.kills.Add(kill);
                                    serverUpdate.Write(player.writer, true);
                                    
                                    break;
                                } catch(Exception) {
                                    //invalid syntax
                                }
                                */
                                break;
                            case "time":
                                try {
                                    int index = parameter.IndexOf(":");
                                    int hour = Convert.ToInt32(parameter.Substring(0, index));
                                    int minute = Convert.ToInt32(parameter.Substring(index + 1));

                                    var time = new InGameTime() {
                                        Time = (hour * 60 + minute) * 60000
                                    };
                                    SendUDP(time.data, source);
                                } catch(Exception) {
                                    //invalid syntax
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    BroadcastUDP(datagram, null, true); //pass to all players
                    break;
                    #endregion
                case Database.DatagramID.time:
                    #region time
                    BroadcastUDP(datagram);
                    break;
                    #endregion
                case Database.DatagramID.interaction:
                    #region interaction
                    BroadcastUDP(datagram, source); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.staticUpdate:
                    break;
                case Database.DatagramID.block:
                    break;
                case Database.DatagramID.particle:
                    break;
                case Database.DatagramID.connect:
                    #region connect
                    Player player = connections[guids[source]];
                    var connect = new Connect(datagram) {
                        Guid = guids[source],
                        Mapseed = 8710 //hardcoded for now
                    };
                    player.online = true;
                    BroadcastUDP(connect.data);
                    break;
                #endregion
                case Database.DatagramID.disconnect:
                    #region disconnect
                    var disconnect = new Disconnect(datagram);
                    connections[disconnect.Guid].playing = false;
                    BroadcastUDP(datagram, source);
                    break;
                #endregion
                default:
                    Console.WriteLine("unknown DatagramID: " + datagram[0]);
                    break;
            }
        }
    }
}
