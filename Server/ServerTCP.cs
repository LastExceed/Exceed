﻿using System;
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
    class ServerTCP {
        public TcpListener listener;
        Dictionary<ulong, Player> players;
        ulong guidCounter = 1;
        ServerUpdate worldUpdate;

        public ServerTCP() //constructor
        {
            listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();
            players = new Dictionary<ulong, Player>();
            worldUpdate = new ServerUpdate();
            ZoxModel arena = JsonConvert.DeserializeObject<ZoxModel>(File.ReadAllText("thing2.zox"));
            arena.Parse(worldUpdate, 8397006, 8396937, 127); //near spawn || 8286952, 8344462, 204 //position of liuk's biome intersection
        }

        public void Listen() {
            Player player = new Player() {
                tcp = listener.AcceptTcpClient()
            };
            new Thread(new ThreadStart(Listen)).Start(); //for every connection a new thread is created to make sure that packets are received asap
            player.writer = new BinaryWriter(player.tcp.GetStream());
            player.reader = new BinaryReader(player.tcp.GetStream());
            int packetID = -1;
            while (player.tcp.Connected) {
                try {
                    packetID = player.reader.ReadInt32();
                } catch (IOException) {
                    Kick(player);
                    break;
                }
                Process_packet(packetID, player);
            }
        }

        public void Process_packet(int packetID, Player player) {
            switch (packetID) {
                case 0:
                    #region entity update
                    var entityUpdate = new EntityUpdate();
                    entityUpdate.Read(player.reader);
                    int bitfield1 = entityUpdate.bitfield1;
                    int bitfield2 = entityUpdate.bitfield2;
                    string ACmessage = AntiCheat.Inspect(entityUpdate);
                    if (ACmessage != "ok") {
                        var kickMessage = new ChatMessage() {
                            message = "illegal " + ACmessage
                        };
                        kickMessage.Send(player);
                        Console.WriteLine(player.entityData.name + " kicked for illegal " + kickMessage.message);
                        Thread.Sleep(100); //thread is about to run out anyway so np
                        Kick(player);
                    }
                    if (Tools.GetBit(entityUpdate.bitfield2, 45 - 32)) {
                        Announce.Join(entityUpdate.name, player.entityData.name, players);
                    }
                    entityUpdate.entityFlags |= 1 << 5; //enable friendly fire flag for pvp
                    entityUpdate.Filter(player.entityData);
                    if (entityUpdate.bitfield1 != 0 || entityUpdate.bitfield2 != 0) {
                        entityUpdate.Send(players, 0);
                        entityUpdate.bitfield1 = bitfield1; //bitfield reverted to unfiltered for future purposes
                        entityUpdate.bitfield2 = bitfield2;
                        if (Tools.GetBit(bitfield1, 27) && entityUpdate.HP == 0 && player.entityData.HP > 0) {
                            Tomb.Show(player).Send(players, 0);
                        } else if (Tools.GetBit(bitfield1, 27) && player.entityData.HP == 0 && entityUpdate.HP > 0) {
                            Tomb.Hide(player).Send(players, 0);
                        }
                        entityUpdate.Merge(player.entityData);
                    }
                    break;
                #endregion
                case 6:
                    #region action
                    EntityAction action = new EntityAction();
                    action.Read(player.reader);
                    switch (action.type) {
                        case 2: //npc shops/talk WIP
                            Console.WriteLine("###");
                            break;

                        case 3: //static interaction
                            //var staticEntity = new part.StaticEntity();
                            //staticEntity.chunkX = action.chunkX;
                            //staticEntity.chunkY = action.chunkY;
                            //staticEntity.id = action.index;
                            //staticEntity.type = 0;
                            //staticEntity.position = player.entityData.position;
                            //staticEntity.rotation = 0;
                            //staticEntity.size.x = 1;
                            //staticEntity.size.y = 1;
                            //staticEntity.size.z = 1;
                            //staticEntity.closed = 0;
                            //staticEntity.time = 1000;
                            //staticEntity.guid = player.entityData.guid;

                            //var serverUpdate = new ServerUpdate();
                            //serverUpdate.statics.Add(staticEntity);
                            //serverUpdate.send(players, 0);
                            break;

                        case 5: //pickup (shouldn't occur since item drops are disabled)
                            break;

                        case 6: //drop (send item back to dropper because dropping is disabled to prevent chatspam)
                            var pickup = new Pickup() {
                                guid = player.entityData.guid,
                                item = action.item
                            };

                            var serverUpdate6 = new ServerUpdate();
                            serverUpdate6.pickups.Add(pickup);
                            serverUpdate6.Send(player);
                            break;

                        case 8: //call pet WIP
                            //var petItem = player.entityData.equipment[(int)Database.Equipment.pet];

                            //var pet = new EntityUpdate();
                            //pet.guid = 2000 + player.entityData.guid;
                            //pet.bitfield1 = 0b00001000_00000000_00100001_10000001;
                            //pet.bitfield2 = 0b00000000_00000000_00110000_00001000;
                            //pet.position = player.entityData.position;
                            //pet.hostility = (int)Database.Hostility.NPC;
                            //pet.entityType = 28;
                            //pet.appearance = player.entityData.appearance;
                            //pet.HP = 999;
                            //pet.parentOwner = (long)player.entityData.guid;
                            //pet.equipment = player.entityData.equipment;
                            //pet.name = "doppelganger";

                            //pet.send(players, 0);
                            break;

                        default:
                            Console.WriteLine("unknown action (" + action.type + ") by " + player.entityData.name);
                            break;
                    }
                    break;
                #endregion
                case 7:
                    #region hit
                    var hit = new Hit();
                    hit.Read(player.reader);

                    var serverUpdate7 = new ServerUpdate();
                    serverUpdate7.hits.Add(hit);
                    serverUpdate7.Send(players, player.entityData.guid);
                    break;
                #endregion
                case 8:
                    #region passiveProc
                    var passiveProc = new PassiveProc();
                    passiveProc.Read(player.reader);

                    var serverUpdate8 = new ServerUpdate();
                    serverUpdate8.passiveProcs.Add(passiveProc);
                    serverUpdate8.Send(players, player.entityData.guid);

                    switch (passiveProc.type) {
                        case (int)Database.Passives.warFrenzy:
                        case (int)Database.Passives.camouflage:
                        case (int)Database.Passives.fireSpark:
                        case (int)Database.Passives.intuition:
                        case (int)Database.Passives.elusivenes:
                        case (int)Database.Passives.swiftness:
                            //nothing particular yet
                            break;

                        case (int)Database.Passives.manashield:
                            var chatMessage6 = new ChatMessage() {
                                message = "manashield: " + passiveProc.modifier,
                                sender = 0
                            };
                            chatMessage6.Send(player);
                            break;

                        case (int)Database.Passives.bulwalk:
                            //client deals with this automatically
                            break;

                        case (int)Database.Passives.poison:
                            if (players.ContainsKey(passiveProc.target))//in case target is a tomb or sth
                            {
                                var poisonDmg = new Hit() {
                                    attacker = passiveProc.source,
                                    target = passiveProc.target,
                                    damage = passiveProc.modifier,
                                    position = players[passiveProc.target].entityData.position,
                                    type = (byte)Database.DamageTypes.normal
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
                case 9:
                    #region shoot
                    var shoot = new Shoot();
                    shoot.Read(player.reader);

                    var serverUpdate9 = new ServerUpdate();
                    serverUpdate9.shoots.Add(shoot);
                    serverUpdate9.Send(players, player.entityData.guid);
                    break;
                #endregion
                case 10:
                    #region chat
                    var chatMessage = new ChatMessage();
                    chatMessage.read(player.reader);
                    chatMessage.sender = player.entityData.guid;

                    if (chatMessage.message.StartsWith("/")) {
                        string parameter = "";
                        string command = chatMessage.message.Substring(1);
                        if (chatMessage.message.Contains(" ")) {
                            int spaceIndex = command.IndexOf(" ");
                            parameter = command.Substring(spaceIndex + 1);
                            command = command.Substring(0, spaceIndex);
                        }
                        Command.Execute(command, parameter, player); //wip
                    } else {
                        chatMessage.Send(players, 0);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("#" + player.entityData.guid + " " + player.entityData.name + ": ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(chatMessage.message);
                    }
                    break;
                #endregion
                case 11:
                    #region chunk discovered
                    var chunk = new Chunk();
                    chunk.Read(player.reader); //currently not doing anything with this
                    break;
                #endregion
                case 12:
                    #region sector discovered
                    var sector = new Sector();
                    sector.Read(player.reader); //currently not doing anything with this
                    break;
                #endregion
                case 17:
                    #region version
                    var version = new ProtocolVersion();
                    version.Read(player.reader);
                    if (version.version != 3) {
                        version.version = 3;
                        version.Send(player);
                        player.tcp.Close();
                    } else {
                        player.entityData.guid = guidCounter;
                        guidCounter++;
                        players.Add(player.entityData.guid, player);

                        var join = new Join() {
                            guid = player.entityData.guid,
                            junk = new byte[0x1168]
                        };
                        join.Send(player);

                        var mapSeed = new MapSeed() {
                            seed = 8710 //seed is hardcoded for now, dont change
                        };
                        mapSeed.Send(player);

                        foreach (KeyValuePair<ulong, Player> entry in players) {
                            if (entry.Key != player.entityData.guid) {
                                entry.Value.entityData.Send(player);
                            }
                        }
                        //Task.Delay(10000).ContinueWith(t => load_world_delayed(player)); //WIP, causes crash when player disconnects before executed
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
            Announce.Leave(player.entityData.name, players);

            var pdc = new EntityUpdate() {
                guid = player.entityData.guid,
                bitfield1 = 0b00001000_00000000_00000000_10000000,
                hostility = 255 //workaround for DC because i dont like packet2
            };
            pdc.Send(players, 0);
        }

        public void Load_world_delayed(Player player) {
            if (players.ContainsKey(player.entityData.guid)) {
                worldUpdate.Send(player);
            }
        }

        public void Poison(ServerUpdate poisonTick, int duration) {
            if (players.ContainsKey(poisonTick.hits[0].target)) {
                poisonTick.hits[0].position = players[poisonTick.hits[0].target].entityData.position;
                poisonTick.Send(players, 0);
                if (duration > 0) {
                    Task.Delay(500).ContinueWith(t => Poison(poisonTick, duration - 500));
                }
            }
        }
    }
}
