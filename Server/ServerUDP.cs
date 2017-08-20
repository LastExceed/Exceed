using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Timers;

using Resources;
using Resources.Datagram;
using Resources.Packet;
using Server.Addon;
using System.Security.Cryptography.X509Certificates;

namespace Server {
    class ServerUDP {
        UdpClient udpListener;
        TcpListener tcpListener;
        Dictionary<ushort, Player> connections = new Dictionary<ushort, Player>();
        public testEntities DB = new testEntities();
        public Timer timer = new Timer(1000 * 10);
        public X509Certificate2 cert;

        public ServerUDP(int port, X509Certificate2 cert = null) {
            this.cert = cert;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            udpListener = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            Task.Factory.StartNew(ListenUDP);
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            Task.Factory.StartNew(ListenTCP);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            //int last = (int)(timer.Interval / 1000);
            //foreach(var item in connections) {
            //    if(item.Value.playing) {
            //        DB.users.First(x => x.username == item.Value.username).playTime += last;
            //    }
            //}
            //DB.SaveChanges();
        }

        public void ListenTCP() {
            while(true) {
                Player player = new Player(tcpListener.AcceptTcpClient(), cert);
                ushort newGuid = 1;
                while(connections.ContainsKey(newGuid)) {//find lowest available guid
                    newGuid++;
                }
                player.entityData.guid = newGuid;

                Database.LoginResponse response = Database.LoginResponse.fail;

                #region secure login
                /*
                string username = player.reader.ReadString(); // Read username

                var row = DB.users.FirstOrDefault(x => x.username == username || x.Email == username); // Get user with username from db

                if(row != null) {
                    player.username = row.username;

                    var hashData = row.hash.Split('$'); // Split hash from db e.g. $CUBEHASH$Version$itterations$hash
                    byte[] hashBytes = Convert.FromBase64String(hashData[4]); //Get hasbytes = salt + hash

                    byte[] salt = new byte[Hashing.saltSize];
                    Array.Copy(hashBytes, 0, salt, 0, salt.Length); // extract salt
                    byte[] hash = new byte[Hashing.hashSize];
                    Array.Copy(hashBytes, salt.Length, hash, 0, hash.Length);// extract hash

                    player.writer.Write(salt); //send salt

                    byte[] clientHash = player.reader.ReadBytes(Hashing.hashSize); // Get clientside hashed password

                    if(hash.SequenceEqual(clientHash)) {
                        if(row.banned.HasValue) {
                            response = Database.LoginResponse.banned;
                        } else {
                            row.lastLoggin = DateTime.Now;
                            connections.Add(newGuid, player);
                            response = Database.LoginResponse.success;
                        }
                    }
                } else {
                    // Advance with random data so bridge dosen't get stuck
                    byte[] bytes = new byte[Hashing.saltSize];
                    new Random().NextBytes(bytes);
                    player.writer.Write(bytes);
                    player.reader.ReadBytes(Hashing.hashSize);
                }*/
                #endregion
                if (player.reader.ReadInt32() == 123) {
                    response = Database.LoginResponse.success;
                    connections.Add(newGuid, player);
                }
                player.writer.Write((byte)response);
            }
        }
        public void ListenUDP() {
            IPEndPoint source = null;
            while(true) {
                byte[] datagram = udpListener.Receive(ref source);
                ProcessDatagram(datagram, connections.First(x => x.Value.Address.Equals(source)).Value);
            }
        }

        public void SendUDP(byte[] data, Player target) {
            udpListener.Send(data, data.Length, target.Address);
        }
        public void BroadcastUDP(byte[] data, Player toSkip = null, bool includeNotPlaying = false) {
            foreach(var player in connections.Values) {
                if(player != toSkip && (player.playing || includeNotPlaying)) {
                    SendUDP(data, player);
                }
            }
        }

        public void ProcessPacket(int packetID, Player player) {
            throw new NotImplementedException();
        }
        public void ProcessDatagram(byte[] datagram, Player player) {
            switch((Database.DatagramID)datagram[0]) {
                case Database.DatagramID.entityUpdate:
                    #region entityUpdate
                    var entityUpdate = new EntityUpdate(datagram);

                    string ACmessage = AntiCheat.Inspect(entityUpdate);
                    if(ACmessage != "ok") {
                        //var kickMessage = new ChatMessage() {
                        //    message = "illegal " + ACmessage
                        //};
                        //kickMessage.Write(player.writer, true);
                        //Console.WriteLine(player.entityData.name + " kicked for illegal " + kickMessage.message);
                        //Thread.Sleep(100); //thread is about to run out anyway so np
                        //Kick(player);
                        //return;
                    }
                    if(entityUpdate.name != null) {
                        //Announce.Join(entityUpdate.name, player.entityData.name, players);
                    }

                    entityUpdate.entityFlags |= 1 << 5; //enable friendly fire flag for pvp
                    if(!player.entityData.IsEmpty) { //dont filter the first packet
                        entityUpdate.Filter(player.entityData);
                    }
                    if(!entityUpdate.IsEmpty) {
                        //entityUpdate.Broadcast(players, 0);
                        BroadcastUDP(entityUpdate.Data, player);
                        if(entityUpdate.HP == 0 && player.entityData.HP > 0) {
                            BroadcastUDP(Tomb.Show(player).Data);
                        } else if(player.entityData.HP == 0 && entityUpdate.HP > 0) {
                            BroadcastUDP(Tomb.Hide(player).Data);
                        }
                        entityUpdate.Merge(player.entityData);
                    }
                    break;
                #endregion
                case Database.DatagramID.attack:
                    #region attack
                    BroadcastUDP(datagram, player); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.shoot:
                    #region shoot
                    BroadcastUDP(datagram, player); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.proc:
                    #region proc
                    BroadcastUDP(datagram, player); //pass to all players except source
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

                                    var xpDummy = new DynamicUpdate() {
                                        guid = 1000,
                                        hostility = (byte)Database.Hostility.enemy
                                    };

                                    BroadcastUDP(xpDummy.GetData());

                                    var kill = new Kill() {
                                        killer = player.entityData.guid,
                                        victim = 1000,
                                        xp = amount
                                    };
                                    var serverUpdate = new ServerUpdate();
                                    serverUpdate.kills.Add(kill);
                                    serverUpdate.Write(player.writer, true);
                                    
                                    break;
                                } catch(Exception) {
                                    SendUDP(new Chat("Wrong Syntax").data, player);
                                }
                                */
                                break;
                            case "time":
                                try {
                                    int index = parameter.IndexOf(":");
                                    int hour = int.Parse(parameter.Substring(0, index));
                                    int minute = int.Parse(parameter.Substring(index + 1));

                                    var time = new InGameTime() {
                                        Time = (hour * 60 + minute) * 60000
                                    };
                                    SendUDP(time.data, player);
                                } catch(Exception) {
                                    SendUDP(new Chat("Wrong Syntax").data, player);
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
                    break; // Ignore time
                case Database.DatagramID.interaction:
                    #region interaction
                    BroadcastUDP(datagram, player); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.staticUpdate:
                case Database.DatagramID.block:
                case Database.DatagramID.particle:
                    break;
                case Database.DatagramID.connect:
                    #region connect
                    var connect = new Connect(datagram) {
                        Guid = (ushort)player.entityData.guid,
                        Mapseed = 8710 //hardcoded for now
                    };
                    SendUDP(connect.data, player);

                    foreach(Player p in connections.Values) {
                        if(p.playing) {
                            SendUDP(p.entityData.Data, player);
                        }
                    }
                    player.playing = true;
                    break;
                #endregion
                case Database.DatagramID.disconnect:
                    #region disconnect
                    var disconnect = new Disconnect(datagram);
                    connections[disconnect.Guid].playing = false;
                    BroadcastUDP(datagram, player);
                    break;
                #endregion
                default:
                    Console.WriteLine("unknown DatagramID: " + datagram[0]);
                    break;
            }
        }
    }
}
