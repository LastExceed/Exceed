﻿using System;
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

namespace Server {
    class ServerUDP {
        UdpClient udpClient;
        TcpListener tcpListener;
        ServerUpdate worldUpdate = new ServerUpdate();
        Dictionary<ushort, Player> players = new Dictionary<ushort, Player>();
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

            Console.WriteLine("loading completed");

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
            if (!accounts.ContainsKey(username)) {
                player.writer.Write((byte)AuthResponse.unknownUser);
                return;
            }
            string password = player.reader.ReadString();
            if (accounts[username] != password) {
                player.writer.Write((byte)AuthResponse.wrongPassword);
                return;
            }
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

            ushort newGuid = 1;
            while (players.ContainsKey(newGuid)) {//find lowest available guid
                newGuid++;
            }
            player.entityData.guid = newGuid;
            players.Add(newGuid, player);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(newGuid + " connected");
            
            while (true) {
                try {
                    byte packetID = player.reader.ReadByte();
                    ProcessPacket(packetID, player);
                } catch (IOException) {
                    players.Remove((ushort)player.entityData.guid);
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
                byte[] datagram = udpClient.Receive(ref source);
                Player p = players.FirstOrDefault(x => x.Value.IpEndPoint.Equals(source)).Value;

                if(p != null) {
                    var asd = Task.Factory.StartNew(() => ProcessDatagram(datagram, p));
                }
            }
        }

        public void SendUDP(byte[] data, Player target) {
            udpClient.Send(data, data.Length, target.IpEndPoint);
        }
        public void BroadcastUDP(byte[] data, Player toSkip = null, bool includeNotPlaying = false) {
            foreach(var player in players.Values) {
                if(player != toSkip && (player.playing || includeNotPlaying)) {
                    SendUDP(data, player);
                }
            }
        }

        public void ProcessPacket(byte packetID, Player player) {
            switch (packetID) {
                case 0://query
                    var query = new Query("Exceed Official", 65535);

                    foreach(var connection in players.Values) {
                        if(connection.playing) {
                            query.players.Add((ushort)connection.entityData.guid, connection.entityData.name);
                        }
                    }
                    query.Write(player.writer);
                    break;
                default:
                    Console.WriteLine("unknown packet: " + packetID);
                    break;
            }
        }
        public void ProcessDatagram(byte[] datagram, Player player) {
            switch ((DatagramID)datagram[0]) {
                case DatagramID.entityUpdate:
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
                case DatagramID.attack:
                    #region attack
                    var attack = new Attack(datagram);
                    player.lastTarget = attack.Target;
                    if (players.ContainsKey(attack.Target)) {//in case the target is a tombstone
                        SendUDP(datagram, players[attack.Target]);
                    }
                    break;
                #endregion
                case DatagramID.shoot:
                    #region shoot
                    var shoot = new Resources.Datagram.Shoot(datagram);
                    BroadcastUDP(datagram, player); //pass to all players except source
                    break;
                #endregion
                case DatagramID.proc:
                    #region proc
                    var proc = new Proc(datagram);

                    switch (proc.Type) {
                        case ProcType.bulwalk:
                            SendUDP(new Chat(string.Format("bulwalk: {0}% dmg reduction", 1.0f - proc.Modifier)).data, player);
                            break;
                        case ProcType.poison:
                            var poisonTick = new Attack() {
                                Damage = proc.Modifier,
                                Target = proc.Target
                            };
                            Poison(players[proc.Target], poisonTick);
                            break;
                        case ProcType.manashield:
                            SendUDP(new Chat(string.Format("manashield: {0}", proc.Modifier)).data, player);
                            break;
                        case ProcType.warFrenzy:
                        case ProcType.camouflage:
                        case ProcType.fireSpark:
                        case ProcType.intuition:
                        case ProcType.elusivenes:
                        case ProcType.swiftness:
                            break;
                        default:

                            break;
                    }
                    BroadcastUDP(datagram, player); //pass to all players except source
                    break;
                #endregion
                case DatagramID.chat:
                    #region chat
                    var chat = new Chat(datagram);
                    if (chat.Text.StartsWith("/")) {
                        string parameter = string.Empty;
                        string command = chat.Text.Substring(1);
                        if (chat.Text.Contains(" ")) {
                            int spaceIndex = command.IndexOf(" ");
                            parameter = command.Substring(spaceIndex + 1);
                            command = command.Substring(0, spaceIndex);
                        }
                        Command.UDP(command, parameter, player, this); //wip
                    } else {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(players[chat.Sender].entityData.name + ": ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(chat.Text);
                        BroadcastUDP(datagram, null, true); //pass to all players
                    }
                    break;
                #endregion
                case DatagramID.interaction:
                    #region interaction
                    var interaction = new Interaction(datagram);
                    BroadcastUDP(datagram, player); //pass to all players except source
                    break;
                #endregion
                case DatagramID.connect:
                    #region connect
                    var connect = new Connect(datagram) {
                        Guid = (ushort)player.entityData.guid,
                        Mapseed = Database.mapseed
                    };
                    SendUDP(connect.data, player);

                    foreach(Player p in players.Values) {
                        if(p.playing) {
                            SendUDP(p.entityData.Data, player);
                        }
                    }
                    player.playing = true;
                    Task.Delay(100).ContinueWith(t => Load_world_delayed(player)); //WIP, causes crash when player disconnects before executed
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(connect.Guid + " is now playing");
                    break;
                #endregion
                case DatagramID.disconnect:
                    #region disconnect
                    var disconnect = new Disconnect(datagram);
                    players[disconnect.Guid].playing = false;
                    BroadcastUDP(datagram, player, true);

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(disconnect.Guid + " is now lurking");
                    break;
                #endregion
                case DatagramID.specialMove:
                    #region specialMove
                    var specialMove = new SpecialMove(datagram);
                    switch (specialMove.Id) {
                        case SpecialMoveID.taunt:
                            var targetGuid = specialMove.Guid;
                            specialMove.Guid = (ushort)player.entityData.guid;
                            SendUDP(specialMove.data, players[targetGuid]);
                            break;
                        case SpecialMoveID.cursedArrow:
                        case SpecialMoveID.arrowRain:
                        case SpecialMoveID.shrapnel:
                        case SpecialMoveID.smokeBomb:
                        case SpecialMoveID.iceWave:
                        case SpecialMoveID.confusion:
                        case SpecialMoveID.shadowStep:
                            BroadcastUDP(specialMove.data, player);
                            break;
                        default:
                            break;
                    }

                    //byte skill = 4;
                    //var otherAction = new SpecialMove(datagram);
                    //switch (skill) {
                    //    case 1:
                    //        #region arrow rain
                    //        var arrowRain = new Resources.Datagram.Shoot {
                    //            Scale = 1
                    //        };
                    //        var ed = connections[otherAction.Guid].entityData;
                    //        var pos = new Resources.Utilities.LongVector() {
                    //            x = ed.position.x + (long)ed.rayHit.x * 0x10000,
                    //            y = ed.position.y + (long)ed.rayHit.y * 0x10000,
                    //            z = ed.position.z + (long)ed.rayHit.z * 0x10000
                    //        };
                    //        pos.x += 0x30000;
                    //        pos.y += 0x30000;
                    //        pos.z += 0x80000;
                    //        for (int i = 0; i < 7; i++) {
                    //            for (int j = 0; j < 7; j++) {
                    //                arrowRain.Position = pos;
                    //                BroadcastUDP(arrowRain.data);
                    //                pos.x += 0x10000;
                    //            }
                    //            pos.x -= 0x70000;
                    //            pos.y += 0x10000;
                    //        }
                    //        break;
                    //    #endregion
                    //    case 2:
                    //        #region bladestorm
                    //        //boomerang projectiles despawn instantly, idk why
                    //        var bladeStorm = new Resources.Datagram.Shoot {
                    //            Scale = 1,
                    //            Projectile = Projectile.boomerang,
                    //            Position = connections[otherAction.Guid].entityData.position
                    //        };
                    //        var vel = new Resources.Utilities.FloatVector();
                    //        for (int i = 0; i < 16; i++) {
                    //            vel.x = 20 * (float)Math.Sin(Math.PI / 8 * i);
                    //            vel.y = 20 * (float)Math.Cos(Math.PI / 8 * i);
                    //            bladeStorm.Velocity = vel;
                    //            BroadcastUDP(bladeStorm.data);
                    //        }
                    //        break;
                    //    #endregion
                    //    case 3:
                    //        #region shrapnel
                    //        var shrapnel = new Resources.Datagram.Shoot {
                    //            Scale = 1,
                    //            Position = connections[otherAction.Guid].entityData.position
                    //        };
                    //        vel = new Resources.Utilities.FloatVector() {
                    //            z = 2
                    //        };
                    //        int density = 0x10000;
                    //        for (int i = 0; i < density; i++) {
                    //            vel.x = 30 * (float)Math.Sin(Math.PI / (density / 2) * i);
                    //            vel.y = 30 * (float)Math.Cos(Math.PI / (density / 2) * i);
                    //            shrapnel.Velocity = vel;
                    //            BroadcastUDP(shrapnel.data);
                    //        }
                    //        break;
                    //    #endregion
                    //    case 4:
                    //        #region blink
                    //        var teleport = new EntityUpdate {
                    //            guid = player.entityData.guid,
                    //            position = connections[player.lastTarget].entityData.position,
                    //            manaCubes = 3
                    //        };
                    //        SendUDP(teleport.Data, player);
                    //        break;
                    //    #endregion
                    //    default:
                    //        break;
                    //}
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
                worldUpdate.Write(player.writer, true);
            } catch { }
        }
        public void Poison(Player target, Attack attack, byte iteration = 0) {
            if (iteration < 7 && players.ContainsValue(target) && target.playing) {
                SendUDP(attack.data, target);
                iteration++;
                Task.Delay(500).ContinueWith(t => Poison(target, attack, iteration));
            }
        }

        public void Ban(ushort guid) {
            var player = players[guid];
            bans.Add(new Ban(player.entityData.name, player.entityData.name, player.MAC));
            player.tcp.Close();
            File.WriteAllText(bansFilePath, JsonConvert.SerializeObject(bans));
        }
    }
}
