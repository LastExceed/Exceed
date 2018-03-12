using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

using Server.Addon;

using Resources;
using Resources.Datagram;
using Resources.Packet;
using Newtonsoft.Json;
using Resources.Utilities;

namespace Server {
    class ServerUDP {
        UdpClient udpClient;
        TcpListener tcpListener;
        List<Player> players = new List<Player>();
        Dictionary<ushort, EntityUpdate> dynamicEntities = new Dictionary<ushort, EntityUpdate>();
        List<Ban> bans;
        Dictionary<string, string> accounts;

        const string bansFilePath = "bans.json";
        const string accountsFilePath = "accounts.json";

        public ServerUDP(int port) {
            if (File.Exists(bansFilePath)) {
                bans = JsonConvert.DeserializeObject<List<Ban>>(File.ReadAllText(bansFilePath));
            }
            else {
                Console.WriteLine("no bans file found");
                bans = new List<Ban>();
                //File.WriteAllText(bansFilePath, JsonConvert.SerializeObject(bans));
            }

            if (File.Exists(accountsFilePath)) {
                accounts = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(accountsFilePath));
            }
            else {
                Console.WriteLine("no accounts file found");
                accounts = new Dictionary<string, string>();
                //File.WriteAllText(accountsFilePath, JsonConvert.SerializeObject(accounts));
            }

            #region models
            //var rnd = new Random();
            //for (int i = 8286946; i < 8286946 + 512; i++) {
            //    for (int j = 8344456; j < 8344456 + 512; j++) {
            //        var block = new ServerUpdate.BlockDelta() {
            //            color = new Resources.Utilities.ByteVector() {
            //                x = 0,
            //                y = 0,
            //                z = (byte)rnd.Next(0, 255),
            //            },
            //            type = BlockType.solid,
            //            position = new Resources.Utilities.IntVector() {
            //                x = i,
            //                y = j,
            //                z = 208,
            //            },
            //        };
            //        worldUpdate.blockDeltas.Add(block);
            //    }
            //}
            //x = 543093329157,
            //y = 546862296355,
            //z = 14423162
            //ZoxModel model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Fulcnix_exceedspawn.zox"));
            //model.Parse(worldUpdate, 8286883, 8344394, 200); 
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_Tavern2.zox"));
            //model.Parse(worldUpdate, 8287010, 8344432, 200);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_Tavern1.zox"));
            //model.Parse(worldUpdate, 8286919, 8344315, 212); 
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/arena/aster_arena.zox"));
            //model.Parse(worldUpdate, 8286775, 8344392, 207);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/michael_project1.zox"));
            //model.Parse(worldUpdate, 8286898, 8344375, 213); 
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/arena/fulcnix_hall.zox"));
            //model.Parse(worldUpdate, 8286885, 8344505, 208); 
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/arena/fulcnix_hall.zox"));
            //model.Parse(worldUpdate, 8286885, 8344629, 208); 
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Tiecz_MountainArena.zox"));
            //model.Parse(worldUpdate, 8286885, 8344759, 208);
            ////8397006, 8396937, 127 //near spawn
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay11.zox"));
            //model.Parse(worldUpdate, 8286770, 8344262, 207);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay12.zox"));
            //model.Parse(worldUpdate, 8286770, 8344136, 207);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay13.zox"));
            //model.Parse(worldUpdate, 8286770, 8344010, 207);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay14.zox"));
            //model.Parse(worldUpdate, 8286770, 8344010, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay01.zox"));
            //model.Parse(worldUpdate, 8286644, 8344010, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay02.zox"));
            //model.Parse(worldUpdate, 8286118, 8344010, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay03.zox"));
            //model.Parse(worldUpdate, 8285992, 8344010, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay04.zox"));
            //model.Parse(worldUpdate, 8285992, 8344136, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay05.zox"));
            //model.Parse(worldUpdate, 8285992, 8344262, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay06.zox"));
            //model.Parse(worldUpdate, 8286118, 8344262, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay07.zox"));
            //model.Parse(worldUpdate, 8286118, 8344136, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay08.zox"));
            //model.Parse(worldUpdate, 8286244, 8344136, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay09.zox"));
            //model.Parse(worldUpdate, 8286244, 8344262, 333);
            //model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_CloudyDay10.zox"));
            //model.Parse(worldUpdate, 8286770, 8344262, 333);
            #endregion

            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            new Thread(new ThreadStart(ListenUDP)).Start();
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            new Thread(new ThreadStart(ListenTCP)).Start();
        }

        public void ListenTCP() {
            var player = new Player(tcpListener.AcceptTcpClient());
            new Thread(new ThreadStart(ListenTCP)).Start();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine((player.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address + " connected");
            try {
                while (true) ProcessPacket(player.reader.ReadByte(), player);
            }
            catch (IOException) {
                if (player.entity != null) {
                    BroadcastUDP(new RemoveDynamicEntity() {
                        Guid = (ushort)player.entity.guid
                    }.data,player);
                    dynamicEntities.Remove((ushort)player.entity.guid);
                }
                players.Remove(player);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine((player.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address + " disconnected");
            }
        }
        public void ListenUDP() {
            IPEndPoint source = null;
            while (true) {
                byte[] datagram = udpClient.Receive(ref source);
                var player = players.FirstOrDefault(x => (x.tcpClient.Client.RemoteEndPoint as IPEndPoint).Equals(source));
                if (player != null && player.entity != null) {
                    ProcessDatagram(datagram, player);
                }
            }
        }

        public void SendUDP(byte[] data, Player target) {
            udpClient.Send(data, data.Length, target.tcpClient.Client.RemoteEndPoint as IPEndPoint);
        }
        public void BroadcastUDP(byte[] data, Player toSkip = null) {
            foreach (var player in players) {
                if (player != toSkip) {
                    SendUDP(data, player);
                }
            }
        }

        public void ProcessPacket(byte packetID, Player player) {
            switch (packetID) {
                case 0://bridge version
                    player.writer.Write((byte)0);
                    if (player.reader.ReadInt32() != Config.bridgeVersion) {
                        player.writer.Write(false);
                        //close connection
                        break;
                    }
                    player.writer.Write(true);
                    players.Add(player);
                    foreach (EntityUpdate entity in dynamicEntities.Values) {
                        SendUDP(entity.CreateDatagram(), player);
                    }
                    break;

                case 1://login
                    #region login
                    player.writer.Write((byte)1);
                    if (!players.Contains(player)) {
                        //musnt login without checking bridge version first
                    }
                    
                    string username = player.reader.ReadString();
                    //if (!accounts.ContainsKey(username)) {
                    //    player.writer.Write((byte)AuthResponse.unknownUser);
                    //    return;
                    //}
                    string password = player.reader.ReadString();
                    //if (accounts[username] != password) {
                    //    player.writer.Write((byte)AuthResponse.wrongPassword);
                    //    return;
                    //}
                    player.writer.Write((byte)AuthResponse.Success);

                    player.MAC = player.reader.ReadString();
                    var banEntry = bans.FirstOrDefault(x => x.Mac == player.MAC || x.Ip == (player.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
                    if (banEntry != null) {
                        player.writer.Write(true);
                        player.writer.Write(banEntry.Reason);
                        break;
                    }
                    player.writer.Write(false);//not banned

                    player.entity = new EntityUpdate() {
                        guid = AssignGuid(),
                    };
                    player.writer.Write((ushort)player.entity.guid);
                    player.writer.Write(Config.mapseed);

                    dynamicEntities.Add((ushort)player.entity.guid, player.entity);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine((player.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address + " logged in as " + username);
                    break;
                #endregion
                case 2:
                    #region logout
                    if (player.entity == null) //not logged in
                    dynamicEntities.Remove((ushort)player.entity.guid);
                    var remove = new RemoveDynamicEntity() {
                        Guid = (ushort)player.entity.guid,
                    };
                    BroadcastUDP(remove.data, player);
                    player.entity = null;
                    if (player.tomb != null) {
                        remove.Guid = (ushort)player.tomb;
                        BroadcastUDP(remove.data, player);
                        player.tomb = null;
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine((player.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address + " logged out");
                    break;
                #endregion
                default:
                    Console.WriteLine("unknown packet: " + packetID);
                    break;
            }
        }
        public void ProcessDatagram(byte[] datagram, Player source) {
            switch ((DatagramID)datagram[0]) {
                case DatagramID.DynamicUpdate:
                    #region entityUpdate
                    var entityUpdate = new EntityUpdate(datagram);
                    #region antiCheat
                    string ACmessage = AntiCheat.Inspect(entityUpdate);
                    if (ACmessage != null) {
                        //kick player
                    }
                    #endregion
                    #region announce
                    if (entityUpdate.name != null) {
                        //Announce.Join(entityUpdate.name, player.entityData.name, players);
                    }
                    #endregion
                    #region pvp
                    entityUpdate.entityFlags |= 1 << 5; //enable friendly fire flag for pvp
                    #endregion
                    #region tombstone
                    if (entityUpdate.HP <= 0 && source.entity.HP > 0) {
                        //entityUpdate.Merge(dynamicEntities[source.guid]);
                        var tombstone = new EntityUpdate() {
                            guid = AssignGuid(),
                            position = source.entity.position,
                            hostility = Hostility.Neutral,
                            entityType = EntityType.None,
                            appearance = new EntityUpdate.Appearance() {
                                character_size = new FloatVector() {
                                    x = 1,
                                    y = 1,
                                    z = 1,
                                },
                                head_model = 2155,
                                head_size = 1
                            },
                            HP = 100,
                            name = "tombstone"
                        };
                        source.tomb = (ushort)tombstone.guid;
                        BroadcastUDP(tombstone.CreateDatagram());
                    }
                    else if (source.entity.HP <= 0 && entityUpdate.HP > 0) {
                        var rde = new RemoveDynamicEntity() {
                            Guid = (ushort)source.tomb,
                        };
                        BroadcastUDP(rde.data);
                    }
                    #endregion
                    entityUpdate.Merge(source.entity);
                    BroadcastUDP(entityUpdate.CreateDatagram(), source);
                    break;
                #endregion
                case DatagramID.Attack:
                    #region attack
                    var attack = new Attack(datagram);
                    source.lastTarget = attack.Target;
                    if (dynamicEntities.ContainsKey(attack.Target) && dynamicEntities[attack.Target].hostility == Hostility.Player) {//in case the target is a tombstone

                        SendUDP(attack.data, players.First(p => p.entity.guid == attack.Target));
                    }
                    var x = new PassiveProc();
                    break;
                #endregion
                case DatagramID.Projectile:
                    #region Projectile
                    var projectile = new Projectile(datagram);
                    BroadcastUDP(projectile.data, source); //pass to all players except source
                    break;
                #endregion
                case DatagramID.Proc:
                    #region proc
                    var proc = new Proc(datagram);
                    BroadcastUDP(proc.data, source); //pass to all players except source
                    break;
                #endregion
                case DatagramID.Chat:
                    #region chat
                    var chat = new Chat(datagram);
                    if (chat.Text.StartsWith("/")) {
                        Command.Server(chat.Text, source, this); //wip
                    }
                    else {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(dynamicEntities[chat.Sender].name + ": ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(chat.Text);
                        BroadcastUDP(chat.data, null); //pass to all players
                    }
                    break;
                #endregion
                case DatagramID.Interaction:
                    #region interaction
                    var interaction = new Interaction(datagram);
                    BroadcastUDP(interaction.data, source); //pass to all players except source
                    break;
                #endregion
                case DatagramID.RemoveDynamicEntity:
                    #region removeDynamicEntity
                    var remove = new RemoveDynamicEntity(datagram);
                    BroadcastUDP(remove.data, source);
                    if (source.tomb != null) {
                        remove.Guid = (ushort)source.tomb;
                        BroadcastUDP(remove.data);
                        source.tomb = null;
                    }
                    source.entity = new EntityUpdate() {
                        guid = source.entity.guid
                    };
                    break;
                #endregion
                case DatagramID.SpecialMove:
                    #region specialMove
                    var specialMove = new SpecialMove(datagram);
                    switch (specialMove.Id) {
                        case SpecialMoveID.Taunt:
                            var target = players.First(p => p.entity.guid == specialMove.Guid);
                            specialMove.Guid = (ushort)source.entity.guid;
                            SendUDP(specialMove.data, target);
                            break;
                        case SpecialMoveID.CursedArrow:
                        case SpecialMoveID.ArrowRain:
                        case SpecialMoveID.Shrapnel:
                        case SpecialMoveID.SmokeBomb:
                        case SpecialMoveID.IceWave:
                        case SpecialMoveID.Confusion:
                        case SpecialMoveID.ShadowStep:
                            BroadcastUDP(specialMove.data, source);
                            break;
                        default:
                            break;
                    }
                    break;
                #endregion
                case DatagramID.HolePunch:
                    break;
                default:
                    Console.WriteLine("unknown DatagramID: " + datagram[0]);
                    break;
            }
        }

        public ushort AssignGuid() {
            ushort newGuid = 1;
            while (dynamicEntities.ContainsKey(newGuid)) newGuid++;
            return newGuid;
        }

        //public void Ban(ushort guid) {
        //    var player = players[guid];
        //    bans.Add(new Ban(dynamicEntities[player.guid].name, player.IpEndPoint.Address.ToString(), player.MAC));
        //    player.tcpClient.Close();
        //    File.WriteAllText(bansFilePath, JsonConvert.SerializeObject(bans));
        //}
    }
}
