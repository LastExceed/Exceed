using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

using Server.Addon;
using Server.Database;

using Microsoft.EntityFrameworkCore;

using Resources;
using Resources.Datagram;
using Resources.Packet;
using Resources.Utilities;

namespace Server {
    public static class Server {
        public static UdpClient udpClient;
        public static TcpListener tcpListener;
        public static List<Player> players = new List<Player>();
        public static Dictionary<ushort, EntityUpdate> dynamicEntities = new Dictionary<ushort, EntityUpdate>();
        public static UserDatabase Database;

        public static void Setup(int port) {
            Database = new UserDatabase();
            Database.Database.Migrate(); //Ensure database exists

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

        private static void ListenTCP() {
            var player = new Player(tcpListener.AcceptTcpClient());
            new Thread(new ThreadStart(ListenTCP)).Start();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(player.IP + " connected");
            try {
                while (true) ProcessPacket(player.reader.ReadByte(), player);
            }
            catch (IOException) {
                if (player.entity != null) {
                    RemovePlayerEntity(player, false);
                }
                players.Remove(player);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(player.IP + " disconnected");
            }
        }
        private static void ListenUDP() {
            IPEndPoint source = null;
            while (true) {
                byte[] datagram = udpClient.Receive(ref source);
                var player = players.FirstOrDefault(x => (x.RemoteEndPoint).Equals(source));
                if (player != null && player.entity != null) {
                    try {
                        ProcessDatagram(datagram, player);
                    }
                    catch (IndexOutOfRangeException) {
                        Kick(player, "invalid data received");
                    }
                }
            }
        }

        private static void SendUDP(byte[] data, Player target) {
            udpClient.Send(data, data.Length, target.RemoteEndPoint);
        }
        private static void BroadcastUDP(byte[] data, Player toSkip = null) {
            foreach (var player in players) {
                if (player != toSkip) {
                    SendUDP(data, player);
                }
            }
        }

        private static void ProcessPacket(byte packetID, Player source) {
            switch ((ServerPacketID)packetID) {
                case ServerPacketID.VersionCheck:
                    #region VersionCheck
                    source.writer.Write((byte)ServerPacketID.VersionCheck);
                    if (source.reader.ReadInt32() != Config.bridgeVersion) {
                        source.writer.Write(false);
                        //close connection
                        break;
                    }
                    source.writer.Write(true);
                    players.Add(source);
                    foreach (EntityUpdate entity in dynamicEntities.Values) {
                        SendUDP(entity.CreateDatagram(), source);
                    }
                    break;
                #endregion
                case ServerPacketID.Login://login
                    #region login
                    string username = source.reader.ReadString();
                    string password = source.reader.ReadString();
                    source.MAC = source.reader.ReadString();

                    AuthResponse authResponse;
                    if (!players.Contains(source)) {
                        //musnt login without checking bridge version first
                        authResponse = AuthResponse.Unverified;
                    }
                    else if (source.entity != null) {
                        //already logged in into another account
                        authResponse = AuthResponse.UserAlreadyLoggedIn;
                    }
                    else if (players.FirstOrDefault(x => x.entity?.name == username) != null) {
                        //another user is already logged into this account
                        authResponse = AuthResponse.AccountAlreadyActive;
                    }
                    else {
                        authResponse = Database.AuthUser(username, password, (int)source.IP.Address, source.MAC);
                    }
                    source.writer.Write((byte)ServerPacketID.Login);
                    source.writer.Write((byte)authResponse);
                    if (authResponse != AuthResponse.Success) break;

                    source.entity = new EntityUpdate() {
                        guid = AssignGuid(),
                        name = username,
                    };
                    source.writer.Write((ushort)source.entity.guid);
                    source.writer.Write(Config.mapseed);

                    dynamicEntities.Add((ushort)source.entity.guid, source.entity);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(source.IP + " logged in as " + username);
                    break;
                #endregion
                case ServerPacketID.Logout:
                    #region logout
                    if (source.entity == null) break;//not logged in
                    RemovePlayerEntity(source, false);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(source.IP + " logged out");
                    break;
                #endregion
                case ServerPacketID.Register:
                    #region Register
                    username = source.reader.ReadString();
                    var email = source.reader.ReadString();
                    password = source.reader.ReadString();

                    RegisterResponse registerResponse;
                    if (!Tools.alphaNumericRegex.IsMatch(username) || !Tools.validEmailRegex.IsMatch(email)) {
                        registerResponse = RegisterResponse.InvalidInput;
                    }
                    else {
                        registerResponse = Database.RegisterUser(username, email, password);
                    }
                    source.writer.Write((byte)ServerPacketID.Register);
                    source.writer.Write((byte)registerResponse);
                    if (registerResponse == RegisterResponse.Success) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(source.IP + " registered: " + username);
                    }
                    break;
                #endregion
                default:
                    Console.WriteLine($"unknown packetID {packetID} received from {source.IP}");
                    source.tcpClient.Close();
                    break;
            }
        }
        private static void ProcessDatagram(byte[] datagram, Player source) {
            switch ((DatagramID)datagram[0]) {
                case DatagramID.DynamicUpdate:
                    #region entityUpdate
                    var entityUpdate = new EntityUpdate(datagram);
                    #region antiCheat
                    string ACmessage = AntiCheat.Inspect(entityUpdate, source.entity);
                    if (ACmessage != null) Kick(source, ACmessage);
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
                    if (entityUpdate.HP <= 0 && (source.entity.HP > 0 || source.entity.HP == null)) {
                        var tombstone = new EntityUpdate() {
                            guid = AssignGuid(),
                            position = entityUpdate.position ?? source.entity.position,
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
                    else if (source.entity.HP <= 0 && entityUpdate.HP > 0 && source.tomb != null) {
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
                    var target = players.FirstOrDefault(p => p.entity?.guid == attack.Target);
                    if (target != null) SendUDP(attack.data, target);
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
                        var parameters = chat.Text.Substring(1).Split(" ");
                        var command = parameters[0].ToLower();
                        switch (command) {
                            case "kick":
                            case "btfo":
                            case "ban":
                                #region ban
                                if (parameters.Length == 1) {
                                    Notify(source, string.Format("usage example: /kick blackrock"));
                                    break;
                                }
                                target = players.FirstOrDefault(x => x.entity.name.Contains(parameters[1]));
                                if (target == null) {
                                    Notify(source, "invalid target");
                                    break;
                                };
                                var reason = "no reason specified";
                                if (parameters.Length > 2) {
                                    reason = parameters[2];
                                }
                                if (command == "kick") {
                                    Kick(target, reason);
                                    break;
                                }
                                target.writer.Write((byte)ServerPacketID.BTFO);
                                target.writer.Write(reason);
                                if (command == "ban") {
                                    Database.BanUser(target.entity.name, (int)target.IP.Address, target.MAC, reason);
                                }
                                RemovePlayerEntity(target, false);
                                break;
                            #endregion
                            case "time":
                                #region time
                                if (parameters.Length == 1) {
                                    Notify(source, string.Format("usage example: /time 12:00"));
                                    break;
                                }
                                var clock = parameters[1].Split(":");
                                if (clock.Length < 2 ||
                                !int.TryParse(clock[0], out int hour) ||
                                !int.TryParse(clock[1], out int minute)) {
                                    Notify(source, string.Format("invalid syntax"));
                                    break;
                                }
                                var inGameTime = new InGameTime() {
                                    Milliseconds = (hour * 60 + minute) * 60000,
                                };
                                SendUDP(inGameTime.data, source);
                                break;
                            #endregion
                            default:
                                Notify(source, string.Format("unknown command '{0}'", parameters[0]));
                                break;
                        }
                        break;
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(dynamicEntities[chat.Sender].name + ": ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(chat.Text);

                    BroadcastUDP(chat.data, null); //pass to all players
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
                    RemovePlayerEntity(source, true);
                    break;
                #endregion
                case DatagramID.SpecialMove:
                    #region specialMove
                    var specialMove = new SpecialMove(datagram);
                    switch (specialMove.Id) {
                        case SpecialMoveID.Taunt:
                            target = players.First(p => p.entity.guid == specialMove.Guid);
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
                    Console.WriteLine($"unknown DatagramID {datagram[0]} received from {source.IP}");
                    Kick(source, "invalid data received");
                    break;
            }
        }

        public static void RemovePlayerEntity(Player player, bool createNewEntity) {
            var rde = new RemoveDynamicEntity() {
                Guid = (ushort)player.entity.guid,
            };
            BroadcastUDP(rde.data, player);
            if (player.tomb != null) {
                rde.Guid = (ushort)player.tomb;
                BroadcastUDP(rde.data);
                player.tomb = null;
            }
            if (createNewEntity) {
                player.entity = new EntityUpdate() {
                    guid = player.entity.guid
                };
            }
            else {
                dynamicEntities.Remove((ushort)player.entity.guid);
                player.entity = null;
            }
        }
        public static void Kick(Player target, string reason) {
            Notify(target, "you got kicked: " + reason);
            target.writer.Write((byte)ServerPacketID.Kick);
            RemovePlayerEntity(target, true);
        }

        public static ushort AssignGuid() {
            ushort newGuid = 1;
            while (dynamicEntities.ContainsKey(newGuid)) newGuid++;
            return newGuid;
        }

        public static void Notify(Player target, string message) {
            var chat = new Chat() {
                Sender = 0,
                Text = message,
            };
            SendUDP(chat.data, target);
        }
    }
}
