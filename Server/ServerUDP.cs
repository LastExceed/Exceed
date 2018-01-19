using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public ServerUpdate worldUpdate = new ServerUpdate();
        Dictionary<ushort, Player> players = new Dictionary<ushort, Player>();
        Dictionary<ushort, EntityUpdate> dynamicEntities = new Dictionary<ushort, EntityUpdate>();
        List<Ban> bans; //MAC|IP 
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
            }

            if (File.Exists(accountsFilePath)) {
                accounts = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(accountsFilePath));
            }
            else {
                Console.WriteLine("no accounts file found");
                accounts = new Dictionary<string, string>();
            }

            #region models
            var rnd = new Random();
            for (int i = 8286946; i < 8286946 + 512; i++) {
                for (int j = 8344456; j < 8344456 + 512; j++) {
                    var block = new ServerUpdate.BlockDelta() {
                        color = new Resources.Utilities.ByteVector() {
                            x = 0,
                            y = 0,
                            z = (byte)rnd.Next(0, 255),
                        },
                        type = BlockType.solid,
                        position = new Resources.Utilities.IntVector() {
                            x = i,
                            y = j,
                            z = 208,
                        },
                    };
                    worldUpdate.blockDeltas.Add(block);
                }
            }
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
            Player player = new Player(tcpListener.AcceptTcpClient());
            new Thread(new ThreadStart(ListenTCP)).Start();

            if (player.reader.ReadInt32() != Database.bridgeVersion) {
                player.writer.Write(false);
                return;
            }
            player.writer.Write(true);

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
            player.writer.Write((byte)AuthResponse.success);
            player.admin = username == "BLACKROCK";

            player.MAC = player.reader.ReadString();
            var banEntry = bans.FirstOrDefault(x => x.Mac == player.MAC || x.Ip == player.IpEndPoint.Address.ToString());
            if (banEntry != null) {
                player.writer.Write(true);
                player.writer.Write(banEntry.Reason);
                return;
            }
            player.writer.Write(false);//not banned

            player.guid = AssignGuid();

            player.writer.Write(player.guid);
            player.writer.Write(Database.mapseed);

            foreach (EntityUpdate entity in dynamicEntities.Values) {
                    SendUDP(entity.CreateDatagram(), player);
            }
            //Task.Delay(100).ContinueWith(t => Load_world_delayed(source)); //WIP, causes crash when player disconnects before executed
            
            players.Add(player.guid, player);
            dynamicEntities.Add(player.guid, new EntityUpdate() {
                guid = player.guid,
            });
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(player.IpEndPoint.Address + " connected");

            while (true) {
                try {
                    byte packetID = player.reader.ReadByte();
                    ProcessPacket(packetID, player);
                }
                catch (IOException) {
                    players.Remove(player.guid);
                    dynamicEntities.Remove(player.guid);
                    var disconnect = new RemoveDynamicEntity() {
                        Guid = (ushort)player.guid
                    };
                    BroadcastUDP(disconnect.data);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(player.IpEndPoint.Address + " disconnected");
                    break;
                }
            }
        }
        public void ListenUDP() {
            IPEndPoint source = null;
            while (true) {
                byte[] datagram = udpClient.Receive(ref source);
                var player = players.FirstOrDefault(x => x.Value.IpEndPoint.Equals(source)).Value;
                if (player != null) {
                    ProcessDatagram(datagram, player);
                }
            }
        }

        public void SendUDP(byte[] data, Player target) {
            udpClient.Send(data, data.Length, target.IpEndPoint);
        }
        public void BroadcastUDP(byte[] data, Player toSkip = null) {
            foreach (var player in players.Values) {
                if (player != toSkip) {
                    SendUDP(data, player);
                }
            }
        }

        public void ProcessPacket(byte packetID, Player source) {
            switch (packetID) {
                case 0://query
                    //var query = new Query("Exceed Official", 65535);

                    //foreach (var player in players.Values) {
                    //    if (player.playing && dynamicEntities[player.guid].name != null) {
                    //        query.players.Add((ushort)player.entityData.guid, player.entityData.name);
                    //    }
                    //}
                    //query.Write(source.writer);
                    break;
                default:
                    Console.WriteLine("unknown packet: " + packetID);
                    break;
            }
        }
        public void ProcessDatagram(byte[] datagram, Player source) {
            switch ((DatagramID)datagram[0]) {
                case DatagramID.dynamicUpdate:
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
                    if (entityUpdate.HP <= 0 && dynamicEntities[source.guid].HP > 0) {
                        //entityUpdate.Merge(dynamicEntities[source.guid]);
                        var tombstone = new EntityUpdate() {
                            guid = AssignGuid(),
                            position = dynamicEntities[source.guid].position,
                            hostility = Hostility.neutral,
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
                    else if (dynamicEntities[source.guid].HP <= 0 && entityUpdate.HP > 0) {
                        var rde = new RemoveDynamicEntity() {
                            Guid = (ushort)source.tomb,
                        };
                        BroadcastUDP(rde.data);
                    }
                    #endregion
                    entityUpdate.Merge(dynamicEntities[source.guid]);
                    BroadcastUDP(entityUpdate.CreateDatagram(), source);
                    break;
                #endregion
                case DatagramID.attack:
                    #region attack
                    var attack = new Attack(datagram);
                    source.lastTarget = attack.Target;
                    if (players.ContainsKey(attack.Target)) {//in case the target is a tombstone
                        SendUDP(datagram, players[attack.Target]);
                    }
                    var x = new PassiveProc();
                    break;
                #endregion
                case DatagramID.shoot:
                    #region shoot
                    var shoot = new Resources.Datagram.Projectile(datagram);
                    BroadcastUDP(datagram, source); //pass to all players except source
                    break;
                #endregion
                case DatagramID.proc:
                    #region proc
                    var proc = new Proc(datagram);

                    switch (proc.Type) {
                        case ProcType.bulwalk:
                            SendUDP(new Chat(string.Format("bulwalk: {0}% dmg reduction", 1.0f - proc.Modifier)).data, source);
                            break;
                        case ProcType.poison:
                            var poisonTickDamage = new Attack() {
                                Damage = proc.Modifier,
                                Target = proc.Target
                            };
                            var target = players[poisonTickDamage.Target];
                            Func<bool> tick = () => {
                                bool f = players.ContainsKey(poisonTickDamage.Target);
                                if (f) {
                                    SendUDP(poisonTickDamage.data, target);
                                }
                                return !f;
                            };
                            Tools.DoLater(tick, 500, 7);
                            //Poison(players[proc.Target], poisonTickDamage);
                            break;
                        case ProcType.manashield:
                            SendUDP(new Chat(string.Format("manashield: {0}", proc.Modifier)).data, source);
                            break;
                        case ProcType.warFrenzy:
                        case ProcType.camouflage:
                        case ProcType.fireSpark:
                        case ProcType.intuition:
                        case ProcType.elusivenes:
                        case ProcType.swiftness:
                            break;
                        default:
                            //unknown proc type
                            break;
                    }
                    BroadcastUDP(datagram, source); //pass to all players except source
                    break;
                #endregion
                case DatagramID.chat:
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
                        BroadcastUDP(datagram, null); //pass to all players
                    }
                    break;
                #endregion
                case DatagramID.interaction:
                    #region interaction
                    var interaction = new Interaction(datagram);
                    BroadcastUDP(datagram, source); //pass to all players except source
                    break;
                #endregion
                case DatagramID.RemoveDynamicEntity:
                    #region disconnect
                    var disconnect = new RemoveDynamicEntity(datagram);
                    BroadcastUDP(datagram, source);
                    dynamicEntities[source.guid] = new EntityUpdate() {
                        guid = source.guid
                    };
                    break;
                #endregion
                case DatagramID.specialMove:
                    #region specialMove
                    var specialMove = new SpecialMove(datagram);
                    switch (specialMove.Id) {
                        case SpecialMoveID.taunt:
                            var targetGuid = specialMove.Guid;
                            specialMove.Guid = source.guid;
                            SendUDP(specialMove.data, players[targetGuid]);
                            break;
                        case SpecialMoveID.cursedArrow:
                        case SpecialMoveID.arrowRain:
                        case SpecialMoveID.shrapnel:
                        case SpecialMoveID.smokeBomb:
                        case SpecialMoveID.iceWave:
                        case SpecialMoveID.confusion:
                        case SpecialMoveID.shadowStep:
                            BroadcastUDP(specialMove.data, source);
                            break;
                        default:
                            break;
                    }
                    break;
                #endregion
                case DatagramID.dummy:
                    break;
                default:
                    Console.WriteLine("unknown DatagramID: " + datagram[0]);
                    break;
            }
        }

        public void Load_world_delayed(Player player) {
            try {
                worldUpdate.Write(player.writer);
            }
            catch { }
        }

        public ushort AssignGuid() {
            ushort newGuid = 1;
            while (dynamicEntities.ContainsKey(newGuid)) newGuid++;
            return newGuid;
        }

        public void Ban(ushort guid) {
            var player = players[guid];
            bans.Add(new Ban(dynamicEntities[player.guid].name, player.IpEndPoint.Address.ToString(), player.MAC));
            player.tcp.Close();
            File.WriteAllText(bansFilePath, JsonConvert.SerializeObject(bans));
        }
    }
}
