using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Addon;

using Resources;
using Resources.Packet;
using Resources.Packet.Part;

namespace Server {
    public class ServerTCP {
        public TcpListener listener;
        Dictionary<long, Player> players = new Dictionary<long, Player>();
        long guidCounter = 1;
        ServerUpdate worldUpdate = new ServerUpdate();

        public ServerTCP(int port) {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Task.Factory.StartNew(Listen);
            Task.Factory.StartNew(CheckLag);
            ZoxModel model = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("models/Aster_SpanishGalleon1670.zox"));
            model.Parse(worldUpdate, 8396876, 8396500, 220); //8397006, 8396937, 127 //near spawn || 8286952, 8344462, 204 //position of liuk's biome intersection
        }

        public void Listen() {
            Player player = new Player(listener.AcceptTcpClient()); //blocks until connection is received
            Task.Factory.StartNew(Listen); //for every connection a new thread is created to make sure that packets are received asap
            int packetID = -1;
            while(player.tcp.Connected) {
                try {
                    packetID = player.reader.ReadInt32();
                    player.lagMeter.Restart();

                    ProcessPacket(packetID, player);
                    player.lagMeter.Restart();
                } catch(Exception ex) {
                    var s = ex.StackTrace;
                    if (ex is IOException || ex is EndOfStreamException) {
                        break;
                    }
                    throw;
                }
            }
            Kick(player);
        }
        public void ProcessPacket(int packetID, Player player) {
            switch((Database.PacketID)packetID) {
                case Database.PacketID.entityUpdate:
                    #region entity update
                    var entityUpdate = new EntityUpdate(player.reader);
                    string ACmessage = AntiCheat.Inspect(entityUpdate);
                    if(ACmessage != "ok") {
                        var kickMessage = new ChatMessage() {
                            message = "illegal " + ACmessage
                        };
                        kickMessage.Write(player.writer, true);
                        Console.WriteLine(player.entityData.name + " kicked for illegal " + kickMessage.message);
                        Thread.Sleep(100); //thread is about to run out anyway so np
                        Kick(player);
                        return;
                    }
                    if(entityUpdate.name != null) {
                        Announce.Join(entityUpdate.name, player.entityData.name, players);
                    }

                    entityUpdate.entityFlags |= 1 << 5; //enable friendly fire flag for pvp
                    if(!player.entityData.IsEmpty) {
                        entityUpdate.Filter(player.entityData);
                    }
                    if(!entityUpdate.IsEmpty) {
                        entityUpdate.Broadcast(players, 0);
                        if(entityUpdate.HP == 0 && player.entityData.HP > 0) {
                            Tomb.Show(player).Broadcast(players, 0);
                        } else if(player.entityData.HP == 0 && entityUpdate.HP > 0) {
                            Tomb.Hide(player).Broadcast(players, 0);
                        }
                        entityUpdate.Merge(player.entityData);
                    }
                    break;
                #endregion
                case Database.PacketID.entityAction:
                    #region action
                    EntityAction entityAction = new EntityAction(player.reader);
                    switch((Database.ActionType)entityAction.type) {
                        case Database.ActionType.talk:
                            break;

                        case Database.ActionType.staticInteraction:
                            //var staticEntity = new StaticEntity();
                            //staticEntity.chunkX = entityAction.chunkX;
                            //staticEntity.chunkY = entityAction.chunkY;
                            //staticEntity.id = entityAction.index;
                            //staticEntity.type = 0;
                            //staticEntity.position = player.entityData.position;
                            //staticEntity.rotation = 0;
                            //staticEntity.size.x = 2;
                            //staticEntity.size.y = 1;
                            //staticEntity.size.z = 1;
                            //staticEntity.closed = 0;
                            //staticEntity.time = 1000;
                            //staticEntity.guid = player.entityData.guid;

                            //var serverUpdate = new ServerUpdate();
                            //serverUpdate.statics.Add(staticEntity);
                            //serverUpdate.Send(players, 0);
                            break;

                        case Database.ActionType.pickup: //shouldn't occur since item drops are disabled
                            break;

                        case Database.ActionType.drop: //send item back to dropper because dropping is disabled to prevent chatspam
                            var pickup = new Pickup() {
                                guid = player.entityData.guid,
                                item = entityAction.item
                            };

                            var serverUpdate6 = new ServerUpdate();
                            serverUpdate6.pickups.Add(pickup);
                            serverUpdate6.Write(player.writer, true);
                            break;

                        case Database.ActionType.callPet:
                            var petItem = player.entityData.equipment[(int)Database.Equipment.pet];

                            var pet = new EntityUpdate() {
                                guid = 2000 + player.entityData.guid,
                                position = player.entityData.position,
                                hostility = (int)Database.Hostility.pet,
                                entityType = 28,
                                appearance = player.entityData.appearance,
                                HP = 999,
                                parentOwner = 3000 + (long)player.entityData.guid,
                                equipment = player.entityData.equipment,
                                name = "doppelganger"
                            };
                            pet.Broadcast(players, 0);

                            pet = new EntityUpdate() {
                                guid = 3000 + player.entityData.guid,
                                position = player.entityData.position,
                                hostility = (int)Database.Hostility.pet,
                                entityType = 28,
                                mode = 106,
                                appearance = player.entityData.appearance,
                                HP = 999,
                                parentOwner = (long)player.entityData.guid,
                                equipment = player.entityData.equipment,
                                name = "doppelganger"
                            };
                            pet.Broadcast(players, 0);
                            break;

                        default:
                            Console.WriteLine("unknown action (" + entityAction.type + ") by " + player.entityData.name);
                            break;
                    }
                    break;
                #endregion
                case Database.PacketID.hit:
                    #region hit
                    var hit = new Hit(player.reader);
                    hit.damage *= 0.75f;
                    if (players.ContainsKey(hit.target)) {
                        var serverUpdate7 = new ServerUpdate();
                        serverUpdate7.hits.Add(hit);
                        serverUpdate7.Broadcast(players, player.entityData.guid);
                    }
                    break;
                #endregion
                case Database.PacketID.passiveProc:
                    #region passiveProc
                    var passiveProc = new PassiveProc(player.reader);

                    var serverUpdate8 = new ServerUpdate();
                    serverUpdate8.passiveProcs.Add(passiveProc);
                    serverUpdate8.Broadcast(players, player.entityData.guid);

                    switch (passiveProc.type)
                    {
                        case (byte)Database.ProcType.warFrenzy:
                        case (byte)Database.ProcType.camouflage:
                        case (byte)Database.ProcType.fireSpark:
                        case (byte)Database.ProcType.intuition:
                        case (byte)Database.ProcType.elusivenes:
                        case (byte)Database.ProcType.swiftness:
                            //nothing particular yet
                            break;

                        case (byte)Database.ProcType.manashield:
                            var chatMessage6m = new ChatMessage()
                            {
                                message = string.Format("manashield: {0}", passiveProc.modifier),
                                sender = 0
                            };
                            chatMessage6m.Write(player.writer, true);
                            break;

                        case (byte)Database.ProcType.bulwalk:
                            var chatMessage6b = new ChatMessage() {
                                message = string.Format("bulwalk: {0}% dmg reduction", 1.0f - passiveProc.modifier),
                                sender = 0
                            };
                            chatMessage6b.Write(player.writer, true);
                            break;

                        case (byte)Database.ProcType.poison:
                            if (players.ContainsKey(passiveProc.target))//in case target is a tomb or sth
                            {
                                var poisonDmg = new Hit()
                                {
                                    attacker = passiveProc.source,
                                    target = passiveProc.target,
                                    damage = passiveProc.modifier,
                                    position = players[passiveProc.target].entityData.position,
                                    type = (byte)Database.DamageType.normal
                                };
                                var poisonTick = new ServerUpdate();
                                poisonTick.hits.Add(poisonDmg);
                                Poison(poisonTick, passiveProc.duration);
                            }
                            break;

                        default:
                            Console.WriteLine("unknown passiveProc.type: " + passiveProc.type);
                            break;
                    }
                    break;
                #endregion
                case Database.PacketID.shoot:
                    #region shoot
                    var shoot = new Shoot(player.reader);

                    var serverUpdate9 = new ServerUpdate();
                    serverUpdate9.shoots.Add(shoot);
                    serverUpdate9.Broadcast(players, player.entityData.guid);
                    break;
                #endregion
                case Database.PacketID.chat:
                    #region chat
                    var chatMessage = new ChatMessage(player.reader) {
                        sender = player.entityData.guid
                    };

                    if(chatMessage.message.StartsWith("/")) {
                        string parameter = "";
                        string command = chatMessage.message.Substring(1);
                        if(chatMessage.message.Contains(" ")) {
                            int spaceIndex = command.IndexOf(" ");
                            parameter = command.Substring(spaceIndex + 1);
                            command = command.Substring(0, spaceIndex);
                        }
                        Command.TCP(command, parameter, player); //wip
                    } else {
                        chatMessage.Broadcast(players, 0);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("#" + player.entityData.guid + " " + player.entityData.name + ": ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(chatMessage.message);
                    }
                    break;
                #endregion
                case Database.PacketID.chunk:
                    #region chunk
                    var chunk = new Chunk(player.reader);
                    break;
                #endregion
                case Database.PacketID.sector:
                    #region sector
                    var sector = new Chunk(player.reader);
                    break;
                #endregion
                case Database.PacketID.version:
                    #region version
                    var version = new ProtocolVersion(player.reader);
                    if(version.version != 3) {
                        version.version = 3;
                        version.Write(player.writer, true);
                        player.tcp.Close();
                    } else {
                        player.entityData.guid = guidCounter;
                        guidCounter++;
                        
                        var join = new Join() {
                            guid = player.entityData.guid,
                            junk = new byte[0x1168]
                        };
                        join.Write(player.writer, true);

                        var mapSeed = new MapSeed() {
                            seed = Database.mapseed
                        };
                        mapSeed.Write(player.writer, true);

                        foreach(Player p in players.Values) {
                            p.entityData.Write(player.writer);
                        }
                        players.Add(player.entityData.guid, player);
                        Task.Delay(10000).ContinueWith(t => Load_world_delayed(player)); //WIP, causes crash when player disconnects before executed
                    }
                    break;
                #endregion
                default:
                    Console.WriteLine("unknown packet ID: " + packetID); //causes some console spam, but allows resyncing with the player without DC or crash
                    break;
            }
        }
        public void Kick(Player player) {
            players.Remove(player.entityData.guid);
            player.tcp.Close();
            player.lagMeter.Stop();
            Announce.Leave(player.entityData.name, players);

            var pdc = new EntityUpdate() {
                guid = player.entityData.guid,
                hostility = 255, //workaround for DC because i dont like packet2
                HP = 0
            };
            pdc.Broadcast(players, 0);
        }

        public void CheckLag() {
            const int lagLimit = 3000;
            while (true) {
                foreach (var player in new List<Player>(players.Values)) {
                    if (player.lagMeter.ElapsedMilliseconds > lagLimit) {
                        new ChatMessage() {
                            message = "lag limit reached"
                        }.Write(player.writer);
                        Thread.Sleep(100);
                        player.tcp.Client.Close();
                    }
                }
                Thread.Sleep(lagLimit);
            }
        }
        public void Load_world_delayed(Player player) {
            try {
                worldUpdate.Write(player.writer, true);
            } catch { }
            
        }
        public void Poison(ServerUpdate poisonTick, int duration) {
            if(players.ContainsKey(poisonTick.hits[0].target)) {
                poisonTick.hits[0].position = players[poisonTick.hits[0].target].entityData.position;
                poisonTick.Broadcast(players, 0);
                if(duration > 0) {
                    Task.Delay(500).ContinueWith(t => Poison(poisonTick, duration - 500));
                }
            }
        }
    }
}
