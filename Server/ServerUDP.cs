using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Timers;

using Resources;
using Resources.Datagram;
using Resources.Packet;
using Server.Addon;
using System.IO;
using Newtonsoft.Json;

namespace Server {
    class ServerUDP {
        UdpClient udpListener;
        TcpListener tcpListener;
        Dictionary<ushort, Player> connections = new Dictionary<ushort, Player>();
        public testEntities DB = new testEntities();
        public Timer timer = new Timer(1000 * 10);
        ServerUpdate worldUpdate = new ServerUpdate();

        public ServerUDP(int port) {
            //timer.Elapsed += Timer_Elapsed;
            //timer.Enabled = true;
            ZoxModel model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/arena/newdii_arena.zox"));
            model.Parse(worldUpdate, 8286952, 8344462, 204); //8397006, 8396937, 127 //near spawn
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
            Player player = new Player(tcpListener.AcceptTcpClient());
            Task.Factory.StartNew(ListenTCP);
            ushort newGuid = 1;
            while(connections.ContainsKey(newGuid)) {//find lowest available guid
                newGuid++;
            }
            player.entityData.guid = newGuid;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(newGuid + " connected");

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

            while (true) {
                try {
                    player.reader.ReadByte(); //replace with handling requests for playerlist ect
                } catch (IOException) {
                    connections.Remove((ushort)player.entityData.guid);
                    var disconnect = new Disconnect() {
                        Guid = (ushort)player.entityData.guid
                    };
                    BroadcastUDP(disconnect.data);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(newGuid + " disconnected");
                    break;
                }
            }
        }
        public void ListenUDP() {
            IPEndPoint source = null;
            while(true) {
                byte[] datagram = udpListener.Receive(ref source);
                try {
                    ProcessDatagram(datagram, connections.First(x => x.Value.Address.Equals(source)).Value);
                } catch (InvalidOperationException) {  }
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
                        //entityUpdate.Filter(player.entityData);
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
                    var attack = new Attack(datagram);
                    SendUDP(datagram, connections[attack.Target]);
                    break;
                #endregion
                case Database.DatagramID.shoot:
                    #region shoot
                    BroadcastUDP(datagram, player); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.proc:
                    #region proc
                    var proc = new Proc(datagram);

                    switch (proc.Type) {
                        case Database.ProcType.bulwalk:
                            SendUDP(new Chat(string.Format("bulwalk: {0}% dmg reduction", 1.0f - proc.Modifier)).data, player);
                            break;
                        case Database.ProcType.poison:
                            var poisonTick = new Attack() {
                                Damage = proc.Modifier,
                               Target = proc.Target
                            };
                            Poison(connections[proc.Target], poisonTick);
                            break;
                        case Database.ProcType.manashield:
                            SendUDP(new Chat(string.Format("manashield: {0}", proc.Modifier)).data, player);
                            break;
                        case Database.ProcType.warFrenzy:
                        case Database.ProcType.camouflage:
                        case Database.ProcType.fireSpark:
                        case Database.ProcType.intuition:
                        case Database.ProcType.elusivenes:
                        case Database.ProcType.swiftness:
                            break;
                        default:

                            break;
                    }
                    BroadcastUDP(datagram, player); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.chat:
                    #region chat
                    var chat = new Chat(datagram);
                    if (chat.Text.StartsWith("/")) {
                        string parameter = "";
                        string command = chat.Text.Substring(1);
                        if (chat.Text.Contains(" ")) {
                            int spaceIndex = command.IndexOf(" ");
                            parameter = command.Substring(spaceIndex + 1);
                            command = command.Substring(0, spaceIndex);
                        }
                        Command.UDP(command, parameter, player, this); //wip
                    } else {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(chat.Sender + ": ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(chat.Text);
                        BroadcastUDP(datagram, null, true); //pass to all players
                    }
                    break;
                #endregion
                case Database.DatagramID.interaction:
                    #region interaction
                    BroadcastUDP(datagram, player); //pass to all players except source
                    break;
                #endregion
                case Database.DatagramID.connect:
                    #region connect
                    var connect = new Connect(datagram) {
                        Guid = (ushort)player.entityData.guid,
                        Mapseed = Database.mapseed
                    };
                    SendUDP(connect.data, player);

                    foreach(Player p in connections.Values) {
                        if(p.playing) {
                            SendUDP(p.entityData.Data, player);
                        }
                    }
                    player.playing = true;
                    Task.Delay(1100).ContinueWith(t => Spawn(player));
                    Task.Delay(10000).ContinueWith(t => Load_world_delayed(player)); //WIP, causes crash when player disconnects before executed
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(connect.Guid + " is now playing");
                    break;
                #endregion
                case Database.DatagramID.disconnect:
                    #region disconnect
                    var disconnect = new Disconnect(datagram);
                    connections[disconnect.Guid].playing = false;
                    BroadcastUDP(datagram, player);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(disconnect.Guid + " is now lurking");
                    break;
                #endregion
                default:
                    Console.WriteLine("unknown DatagramID: " + datagram[0]);
                    break;
            }
        }

        public void Load_world_delayed(Player player) {
            try {
                worldUpdate.Write(player.writer, true);
            } catch { }

        }
        public void Spawn(Player player) {
            var entityUpdate = new EntityUpdate() {
                guid = player.entityData.guid,
                position = new Resources.Utilities.LongVector() {
                    x = (long)65536 * 8286952,
                    y = (long)65536 * 8344462,
                    z = (long)65536 * 204
                }
            };
            SendUDP(entityUpdate.Data, player);
        }
        public void Poison(Player target, Attack attack, byte iteration = 0) {
            if (iteration < 7 && connections.ContainsValue(target) && target.playing) {
                SendUDP(attack.data, target);
                iteration++;
                Task.Delay(500).ContinueWith(t => Poison(target, attack, iteration));
            }

        }
    }
}
