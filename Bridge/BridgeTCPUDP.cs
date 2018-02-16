﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

using Resources;
using Resources.Packet;
using Resources.Datagram;
using System.Threading.Tasks;

namespace Bridge {
    static class BridgeTCPUDP {
        public static UdpClient udpToServer;
        public static TcpClient tcpToServer, tcpToClient;
        public static TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345); //hardcoded because client port can't be changed
        public static BinaryWriter swriter, cwriter;
        public static BinaryReader sreader, creader;
        public static ushort guid;
        public static int mapseed;
        public static FormMain form;
        public static Dictionary<long, EntityUpdate> dynamicEntities = new Dictionary<long, EntityUpdate>();
        public static ushort lastTarget;
        public static Queue<Packet> outgoing = new Queue<Packet>();
        public static BridgeStatus status = BridgeStatus.Offline;

        public static void Connect() {
            form.Log("connecting...", Color.DarkGray);
            string serverIP = Config.serverIP;
            int serverPort = Config.serverPort;

            try {
                tcpToServer = new TcpClient();
                tcpToServer.Connect(serverIP, serverPort);

                udpToServer = new UdpClient(tcpToServer.Client.LocalEndPoint as IPEndPoint);
                udpToServer.Connect(serverIP, serverPort);
                SendUDP(new byte[1] { (byte)DatagramID.HolePunch });
            }
            catch (SocketException) {//connection refused
                Disconnect();
                form.Log("failed\n", Color.Red);
                var result = MessageBox.Show("Unable to connect to Exceed. Please try again later.", "Connection Error", MessageBoxButtons.RetryCancel);
                if (result == DialogResult.Retry) Connect();
                return;
            }
            Stream stream = tcpToServer.GetStream();
            swriter = new BinaryWriter(stream);
            sreader = new BinaryReader(stream);
            form.Log("connected\n", Color.Green);
            new Thread(new ThreadStart(ListenFromServerTCP)).Start();
            status = BridgeStatus.Connected;

            form.Log("checking version...", Color.DarkGray);
            swriter.Write((byte)0);//packetID
            swriter.Write(Config.bridgeVersion);
        }
        public static void Disconnect() {
            status = BridgeStatus.Offline;
            form.Invoke(new Action(() => form.listBoxPlayers.Items.Clear()));
            LingerOption lingerOption = new LingerOption(true, 0);
            try {
                udpToServer.Close();
                udpToServer = null;
            }
            catch { }
            try {
                tcpToServer.LingerState = lingerOption;
                tcpToServer.Client.Close();
                tcpToServer.Close();
                udpToServer = null;
            }
            catch { }
            try {
                tcpToClient.LingerState = lingerOption;
                tcpToClient.Client.Close();
                tcpToClient.Close();
                udpToServer = null;
            }
            catch { }
            dynamicEntities.Clear();
        }

        public static void Login() {
            form.Log("logging in...", Color.DarkGray);
            swriter.Write((byte)1);
            swriter.Write(form.textBoxUsername.Text);
            swriter.Write(form.textBoxPassword.Text);
            swriter.Write(NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault());
        }
        public static void Logout() {
            swriter.Write((byte)2);
            if (status == BridgeStatus.Playing) tcpToClient.Close();
            status = BridgeStatus.Connected;
            form.Log("logged out.\n", Color.DarkGray);
        }

        public static void ListenFromClientTCP() {
            try {
                tcpListener.Start();
            }
            catch (SocketException ex) {
                var result = MessageBox.Show(ex.Message + "\n\nCan't start listening for the client, probably because the CubeWorld default port (12345) is already in use by another program. Do you have a CubeWorld server or another instance of the bridge already running on your computer?\n\nIf you don't know how to fix this, restarting your computer will likely help", "Error", MessageBoxButtons.RetryCancel);
                if (result == DialogResult.Retry) ListenFromClientTCP();
                return;
            }

            while (true) {
                tcpToClient = tcpListener.AcceptTcpClient();
                form.Log("client connected\n", Color.Green);

                tcpToClient.NoDelay = true;
                Stream stream = tcpToClient.GetStream();
                creader = new BinaryReader(stream);
                cwriter = new BinaryWriter(stream);

                if (status != BridgeStatus.LoggedIn) {
                    new ProtocolVersion() {
                        version = 1337
                    }.Write(cwriter);
                    form.Log("client rejected\n", Color.Red);
                }
                else {
                    status = BridgeStatus.Playing;
                    new Thread(new ThreadStart(WriteToClientTCP)).Start();
                    try {
                        while (true) ProcessClientPacket(creader.ReadInt32());
                    }
                    catch (Exception ex) {
                        if (!(ex is IOException || ex is ObjectDisposedException)) throw;
                        switch (status) {
                            case BridgeStatus.Offline://server crashed
                                break;
                            case BridgeStatus.Connected://player logged out
                                break;
                            case BridgeStatus.LoggedIn://impossible
                                goto default;
                            case BridgeStatus.Playing: //client disconnected himself
                                status = BridgeStatus.LoggedIn;
                                SendUDP(new RemoveDynamicEntity() { Guid = guid }.data);
                                break;
                            default:
                                //this shouldnt happen
                                break;
                        }
                        dynamicEntities.Remove(guid);
                        form.Log("client disconnected\n", Color.Red);
                        RefreshPlayerlist();
                        break;
                    }
                }
            }
        }
        public static void ListenFromServerTCP() {
            try {
                while (true) ProcessServerPacket(sreader.ReadByte()); //we can use byte here because it doesn't contain vanilla packets
            }
            catch (IOException) {
                form.Log("Connection to Server lost\n", Color.Red);
                Disconnect();
                Connect();
            }
        }
        public static void ListenFromServerUDP() {
            while (true) {
                IPEndPoint source = null;
                byte[] datagram = null;
                try {
                    datagram = udpToServer.Receive(ref source);
                }
                catch (SocketException) {//when UDPclient is closed
                    return;
                }
                ProcessDatagram(datagram);
            }
        }

        public static void WriteToClientTCP() {
            while (status == BridgeStatus.Playing) {
                if (outgoing.Count != 0) {
                    outgoing.Dequeue().Write(cwriter);
                }
            }
        }

        public static void ProcessDatagram(byte[] datagram) {
            var serverUpdate = new ServerUpdate();
            bool writeServerUpdate = false;
            switch ((DatagramID)datagram[0]) {
                case DatagramID.DynamicUpdate:
                    #region entityUpdate
                    var entityUpdate = new EntityUpdate(datagram);
                    if (status == BridgeStatus.Playing) {
                        if (entityUpdate.guid == guid) {
                            CwRam.Teleport(entityUpdate.position);
                            break;
                        }
                        outgoing.Enqueue(entityUpdate);
                    }

                    if (dynamicEntities.ContainsKey(entityUpdate.guid)) {
                        entityUpdate.Merge(dynamicEntities[entityUpdate.guid]);
                    }
                    else {
                        dynamicEntities.Add(entityUpdate.guid, entityUpdate);
                    }

                    if (entityUpdate.name != null) {
                        RefreshPlayerlist();
                    }
                    break;
                #endregion
                case DatagramID.Attack:
                    #region attack
                    var attack = new Attack(datagram);
                    CwRam.Knockback(attack.Direction);
                    var hit = new Hit() {
                        target = attack.Target,
                        damage = attack.Damage,
                        direction = attack.Direction,
                        critical = attack.Critical,
                        stuntime = attack.Stuntime,
                        position = dynamicEntities[attack.Target].position,
                        isYellow = attack.Skill,
                        type = attack.Type,
                        showlight = attack.ShowLight,
                    };
                    serverUpdate.hits.Add(hit);
                    writeServerUpdate = true;
                    break;
                #endregion
                case DatagramID.Projectile:
                    #region Projectile
                    var projectile = new Projectile(datagram);

                    var shoot = new Shoot() {
                        attacker = projectile.Source,
                        position = projectile.Position,
                        velocity = projectile.Velocity,
                        scale = projectile.Scale,
                        particles = projectile.Particles,
                        projectile = projectile.Type,
                        chunkX = (int)projectile.Position.x / 0x1000000,
                        chunkY = (int)projectile.Position.y / 0x1000000
                    };
                    var angle = Math.Atan2(shoot.velocity.y, shoot.velocity.x);
                    shoot.position.x += (long)(Math.Cos(angle) * 0x10000);
                    shoot.position.y += (long)(Math.Sin(angle) * 0x10000);

                    serverUpdate.shoots.Add(shoot);
                    writeServerUpdate = true;
                    break;
                #endregion
                case DatagramID.Proc:
                    #region proc
                    var proc = new Proc(datagram);
                    if (proc.Type == ProcType.Poison && proc.Target == guid) {
                        var su = new ServerUpdate();
                        su.hits.Add(new Hit() {
                            damage = proc.Modifier,
                            target = guid,
                            position = dynamicEntities[guid].position,
                        });
                        bool tick() {
                            bool f = status == BridgeStatus.Playing && dynamicEntities[guid].HP > 0;
                            if (f) {
                                outgoing.Enqueue(su);
                            }
                            return !f;
                        }
                        Tools.DoLater(tick, 500, 7);
                    }
                    var passiveProc = new PassiveProc() {
                        target = proc.Target,
                        type = proc.Type,
                        modifier = proc.Modifier,
                        duration = proc.Duration
                    };
                    serverUpdate.passiveProcs.Add(passiveProc);
                    writeServerUpdate = true;
                    break;
                #endregion
                case DatagramID.Chat:
                    #region chat
                    var chat = new Chat(datagram);
                    var chatMessage = new ChatMessage() {
                        sender = chat.Sender,
                        message = chat.Text
                    };
                    if (status == BridgeStatus.Playing) outgoing.Enqueue(chatMessage);
                    if (chat.Sender == 0) {
                        form.Log(chat.Text + "\n", Color.Magenta);
                    }
                    else {
                        form.Log(dynamicEntities[chat.Sender].name + ": ", Color.Cyan);
                        form.Log(chat.Text + "\n", Color.White);
                    }
                    break;
                #endregion
                case DatagramID.Time:
                    #region time
                    var igt = new InGameTime(datagram);

                    var time = new Time() {
                        time = igt.Time
                    };
                    if (status == BridgeStatus.Playing) outgoing.Enqueue(time);
                    break;
                #endregion
                case DatagramID.Interaction:
                    #region interaction
                    var interaction = new Interaction(datagram);
                    var entityAction = new EntityAction() {
                        chunkX = interaction.ChunkX,
                        chunkY = interaction.ChunkY,
                        index = interaction.Index,
                        type = ActionType.StaticInteraction
                    };
                    writeServerUpdate = true;
                    break;
                #endregion
                case DatagramID.StaticUpdate:
                    #region staticUpdate
                    var staticUpdate = new StaticUpdate(datagram);

                    var staticEntity = new ServerUpdate.StaticEntity() {
                        chunkX = (int)(staticUpdate.Position.x / (65536 * 256)),
                        chunkY = (int)(staticUpdate.Position.y / (65536 * 256)),
                        id = staticUpdate.Id,
                        type = staticUpdate.Type,
                        position = staticUpdate.Position,
                        rotation = (int)staticUpdate.Direction,
                        size = staticUpdate.Size,
                        closed = staticUpdate.Closed,
                        time = staticUpdate.Time,
                        guid = staticUpdate.User
                    };
                    serverUpdate.statics.Add(staticEntity);
                    writeServerUpdate = true;
                    break;
                #endregion
                case DatagramID.Block:
                    //var block = new Block(datagram);
                    //TODO
                    break;
                case DatagramID.Particle:
                    #region particle
                    var particleDatagram = new Particle(datagram);

                    var particleSubPacket = new ServerUpdate.Particle() {
                        position = particleDatagram.Position,
                        velocity = particleDatagram.Velocity,
                        color = new Resources.Utilities.FloatVector() {
                            x = particleDatagram.Color.R / 255,
                            y = particleDatagram.Color.G / 255,
                            z = particleDatagram.Color.B / 255
                        },
                        alpha = particleDatagram.Color.A / 255,
                        size = particleDatagram.Size,
                        count = particleDatagram.Count,
                        type = particleDatagram.Type,
                        spread = particleDatagram.Spread
                    };
                    serverUpdate.particles.Add(particleSubPacket);
                    break;
                #endregion
                case DatagramID.RemoveDynamicEntity:
                    #region RemoveDynamicEntity
                    var rde = new RemoveDynamicEntity(datagram);
                    entityUpdate = new EntityUpdate() {
                        guid = rde.Guid,
                        hostility = (Hostility)255, //workaround for DC because i dont like packet2
                        HP = 0
                    };
                    if (status == BridgeStatus.Playing) outgoing.Enqueue(entityUpdate);
                    dynamicEntities.Remove(rde.Guid);
                    RefreshPlayerlist();
                    break;
                #endregion
                case DatagramID.SpecialMove:
                    #region speicalMove
                    var specialMove = new SpecialMove(datagram);
                    switch (specialMove.Id) {
                        case SpecialMoveID.Taunt:
                            if (dynamicEntities.ContainsKey(specialMove.Guid)) {
                                if (status == BridgeStatus.Playing) {
                                    CwRam.Teleport(dynamicEntities[specialMove.Guid].position);
                                    CwRam.Freeze(5000);
                                }
                            }
                            break;
                        case SpecialMoveID.CursedArrow:
                            break;
                        case SpecialMoveID.ArrowRain:
                            break;
                        case SpecialMoveID.Shrapnel:
                            break;
                        case SpecialMoveID.SmokeBomb:
                            serverUpdate.particles.Add(new ServerUpdate.Particle() {
                                count = 1000,
                                spread = 5f,
                                type = ParticleType.NoGravity,
                                size = 5f,
                                velocity = new Resources.Utilities.FloatVector(),
                                color = new Resources.Utilities.FloatVector() {
                                    x = 1f,
                                    y = 1f,
                                    z = 1f
                                },
                                alpha = 1f,
                                position = dynamicEntities[specialMove.Guid].position
                            });
                            writeServerUpdate = true;
                            break;
                        case SpecialMoveID.IceWave:
                            break;
                        case SpecialMoveID.Confusion:
                            break;
                        case SpecialMoveID.ShadowStep:
                            break;
                        default:
                            break;
                    }
                    break;
                #endregion
                default:
                    form.Log("unknown datagram ID: " + datagram[0], Color.Red);
                    break;
            }
            if (status == BridgeStatus.Playing && writeServerUpdate) outgoing.Enqueue(serverUpdate);
        }
        public static void ProcessClientPacket(int packetID) {
            switch ((PacketID)packetID) {
                case PacketID.EntityUpdate:
                    #region entityUpdate
                    var entityUpdate = new EntityUpdate(creader);
                    if (dynamicEntities.ContainsKey(entityUpdate.guid)) {
                        entityUpdate.Filter(dynamicEntities[entityUpdate.guid]);
                        entityUpdate.Merge(dynamicEntities[entityUpdate.guid]);
                    }
                    else {
                        dynamicEntities.Add(entityUpdate.guid, entityUpdate);
                    }
                    if (entityUpdate.name != null) {
                        RefreshPlayerlist();
                    }
                    if (!entityUpdate.IsEmpty) {
                        SendUDP(entityUpdate.CreateDatagram());
                    }
                    break;
                #endregion
                case PacketID.EntityAction:
                    #region entity action
                    EntityAction entityAction = new EntityAction(creader);
                    switch (entityAction.type) {
                        case ActionType.Talk:
                            break;
                        case ActionType.StaticInteraction:
                            ChatMessage x = new ChatMessage() {
                                message = "You can't use this, your hands are too small.",
                                sender = 0
                            };
                            outgoing.Enqueue(x);
                            break;
                        case ActionType.PickUp:
                            break;
                        case ActionType.Drop: //send item back to dropper because dropping is disabled to prevent chatspam
                            if (form.radioButtonDestroy.Checked) {
                                outgoing.Enqueue(new ChatMessage() {
                                    message = "item destroyed",
                                    sender = 0,
                                });
                            }
                            else {
                                var serverUpdate = new ServerUpdate();
                                var pickup = new ServerUpdate.Pickup() {
                                    guid = guid,
                                    item = entityAction.item
                                };
                                serverUpdate.pickups.Add(pickup);
                                if (form.radioButtonDuplicate.Checked) {
                                    serverUpdate.pickups.Add(pickup);
                                }
                                outgoing.Enqueue(serverUpdate);
                            }
                            break;
                        case ActionType.CallPet:
                            break;
                        default:
                            //unknown type
                            break;
                    }
                    break;
                #endregion
                case PacketID.Hit:
                    #region hit
                    var hit = new Hit(creader);
                    var attack = new Attack() {
                        Target = (ushort)hit.target,
                        Damage = hit.damage,
                        Direction = hit.direction,
                        Stuntime = hit.stuntime,
                        Skill = hit.isYellow,
                        Type = hit.type,
                        ShowLight = hit.showlight,
                        Critical = hit.critical
                    };
                    SendUDP(attack.data);
                    lastTarget = attack.Target;
                    break;
                #endregion
                case PacketID.PassiveProc:
                    #region passiveProc
                    var passiveProc = new PassiveProc(creader);
                    switch (passiveProc.type) {
                        case ProcType.Bulwalk:
                            outgoing.Enqueue(new ChatMessage() {
                                message = string.Format("bulwalk: {0}% dmg reduction", 1.0f - passiveProc.modifier),
                                sender = 0,
                            });
                            break;
                        case ProcType.WarFrenzy:
                            break;
                        case ProcType.Camouflage:
                            break;
                        case ProcType.Poison:
                            break;
                        case ProcType.UnknownA:
                            break;
                        case ProcType.ManaShield:
                            outgoing.Enqueue(new ChatMessage() {
                                message = string.Format("manashield: {0}", passiveProc.modifier),
                                sender = 0,
                            });
                            break;
                        case ProcType.UnknownB:
                            break;
                        case ProcType.UnknownC:
                            break;
                        case ProcType.FireSpark:
                            break;
                        case ProcType.Intuition:
                            break;
                        case ProcType.Elusiveness:
                            break;
                        case ProcType.Swiftness:
                            break;
                        default:
                            break;
                    }
                    var proc = new Proc() {
                        Target = (ushort)passiveProc.target,
                        Type = passiveProc.type,
                        Modifier = passiveProc.modifier,
                        Duration = passiveProc.duration
                    };
                    SendUDP(proc.data);
                    break;
                #endregion
                case PacketID.Shoot:
                    #region shoot
                    var shoot = new Shoot(creader);
                    var projectile = new Projectile() {
                        Position = shoot.position,
                        Velocity = shoot.velocity,
                        Scale = shoot.scale,
                        Particles = shoot.particles,
                        Type = shoot.projectile,
                        Source = (ushort)shoot.attacker,
                    };
                    SendUDP(projectile.data);
                    break;
                #endregion
                case PacketID.Chat:
                    #region chat
                    var chatMessage = new ChatMessage(creader);
                    if (chatMessage.message.ToLower() == @"/plane") {
                        Console.Beep();
                        var serverUpdate = new ServerUpdate() {
                            blockDeltas = VoxModel.Parse("model.vox"),
                        };
                        foreach (var block in serverUpdate.blockDeltas) {
                            block.position.x += 8286946;
                            block.position.y += 8344456;
                            block.position.z += 220;
                        }

                        outgoing.Enqueue(serverUpdate);
                    }
                    else {
                        var chat = new Chat(chatMessage.message) {
                            Sender = guid//client doesn't send this //(ushort)chatMessage.sender
                        };
                        SendUDP(chat.data);
                    }
                    break;
                #endregion
                case PacketID.Chunk:
                    #region chunk
                    var chunk = new Chunk(creader);
                    break;
                #endregion
                case PacketID.Sector:
                    #region sector
                    var sector = new Sector(creader);
                    break;
                #endregion
                case PacketID.Version:
                    #region version
                    var version = new ProtocolVersion(creader);
                    if (version.version != 3) {
                        version.version = 3;
                        outgoing.Enqueue(version);
                    }
                    else {
                        outgoing.Enqueue(new Join() {
                            guid = guid,
                            junk = new byte[0x1168]
                        });
                        //outgoing.Enqueue(new MapSeed() {
                        //    seed = mapseed
                        //});
                        foreach (var dynamicEntity in dynamicEntities.Values.ToList()) {
                            outgoing.Enqueue(dynamicEntity);
                        }
                    }
                    break;
                #endregion
                default:
                    form.Log("unknown client packet\n", Color.Magenta);
                    break;
            }
        }
        public static void ProcessServerPacket(int packetID) {
            switch (packetID) {
                case 0:
                    if (!sreader.ReadBoolean()) {
                        form.Log("mismatch\n", Color.Red);
                        MessageBox.Show("your bridge is outdated\nupdate\nstay offline");
                        return;
                    }
                    form.Log("match\n", Color.Green);
                    form.Invoke(new Action(() => form.buttonLogin.Enabled = true));
                    new Thread(new ThreadStart(ListenFromServerUDP)).Start();
                    break;

                case 1:
                    switch ((AuthResponse)sreader.ReadByte()) {
                        case AuthResponse.Success:
                            if (sreader.ReadBoolean()) {//if banned
                                MessageBox.Show(sreader.ReadString());//ban message
                                form.Log("you are banned\n", Color.Red);
                                goto default;
                            }
                            form.Log("success\n", Color.Green);
                            break;
                        case AuthResponse.UnknownUser:
                            form.Log("username does not exist\n", Color.Red);
                            goto default;
                        case AuthResponse.WrongPassword:
                            form.Log("wrong password\n", Color.Red);
                            goto default;
                        default:
                            form.Invoke(new Action(() => form.buttonLogin.Enabled = true));
                            return;
                    }
                    status = BridgeStatus.LoggedIn;
                    guid = sreader.ReadUInt16();
                    mapseed = sreader.ReadInt32();

                    form.Invoke(new Action(() => form.buttonLogout.Enabled = true));
                    break;

                //case 3:
                //    var query = new Query(sreader);
                //    foreach (var item in query.players) {
                //        if (!dynamicEntities.ContainsKey(item.Key)) {
                //            dynamicEntities.Add(item.Key, new EntityUpdate());
                //        }
                //        dynamicEntities[item.Key].guid = item.Key;
                //        dynamicEntities[item.Key].name = item.Value;
                //    }
                //    form.Invoke(new Action(form.listBoxPlayers.Items.Clear));
                //    foreach (var playerData in dynamicEntities.Values) {
                //        form.Invoke(new Action(() => form.listBoxPlayers.Items.Add(playerData.name)));
                //    }
                //    break;

                //case 4:
                //    outgoing.Enqueue(new ServerUpdate(sreader));
                //    break;

                default:
                    MessageBox.Show("unknown server packet received");
                    break;
            }
        }

        public static void RefreshPlayerlist() {
            form.Invoke((Action)form.listBoxPlayers.Items.Clear);
            foreach (var dynamicEntity in dynamicEntities.Values.ToList()) {
                if (dynamicEntity.hostility == Hostility.Player) {
                    form.Invoke(new Action(() => form.listBoxPlayers.Items.Add(dynamicEntity.name)));
                }
            }
        }

        public static void OnHotkey(int hotkeyID) {
            HotkeyID hotkey = (HotkeyID)hotkeyID;
            if (hotkey == HotkeyID.TeleportToTown) {
                CwRam.SetMode(Mode.Teleport_To_City, 0);
                return;
            }

            bool spec = dynamicEntities[guid].specialization == 1;
            bool space = hotkeyID == 1;
            switch ((EntityClass)dynamicEntities[guid].entityClass) {
                case EntityClass.Rogue when spec:
                    #region ninja
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region dash
                        CwRam.SetMode(Mode.Spin_Run, 0);
                        #endregion
                        break;
                    }
                    #region blink
                    if (dynamicEntities.ContainsKey(lastTarget)) {
                        CwRam.Teleport(dynamicEntities[guid].position);
                    }
                    #endregion
                    break;
                #endregion
                case EntityClass.Rogue:
                    #region assassin
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region confusion
                        var specialMove = new SpecialMove() {
                            Guid = guid,
                            Id = SpecialMoveID.Confusion,
                        };
                        SendUDP(specialMove.data);
                        #endregion
                    }
                    else {
                        #region shadow step
                        var specialMove = new SpecialMove() {
                            Guid = guid,
                            Id = SpecialMoveID.ShadowStep,
                        };
                        SendUDP(specialMove.data);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Warrior when spec:
                    #region guardian
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region taunt
                        var specialMove = new SpecialMove() {
                            Guid = lastTarget,
                            Id = SpecialMoveID.Taunt,
                        };
                        SendUDP(specialMove.data);
                        #endregion
                    }
                    else {
                        #region steel wall
                        CwRam.SetMode(Mode.Boss_Skill_Block, 0);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Warrior:
                    #region berserk
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region boulder toss
                        CwRam.SetMode(Mode.Boulder_Toss, 0);
                        #endregion
                    }
                    else {
                        #region earth shatter
                        CwRam.SetMode(Mode.Earth_Shatter, 0);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Mage when spec:
                    #region watermage
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region splash
                        CwRam.SetMode(Mode.Splash, 0);
                        #endregion
                    }
                    else {
                        #region ice wave
                        //TODO
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Mage:
                    #region firemage
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region lava
                        CwRam.SetMode(Mode.Lava, 0);
                        #endregion
                    }
                    else {
                        #region beam
                        CwRam.SetMode(Mode.FireRay, 0);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Ranger when spec:
                    #region scout
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region shrapnel
                        //TODO
                        #endregion
                    }
                    else {
                        #region smoke bomb
                        var specialMove = new SpecialMove() {
                            Guid = guid,
                            Id = SpecialMoveID.SmokeBomb,
                        };
                        SendUDP(specialMove.data);

                        var fakeSmoke = new ServerUpdate();
                        fakeSmoke.particles.Add(new ServerUpdate.Particle() {
                            count = 1000,
                            spread = 5f,
                            type = ParticleType.NoGravity,
                            size = 0.3f,
                            velocity = new Resources.Utilities.FloatVector(),
                            color = new Resources.Utilities.FloatVector() {
                                x = 1f,
                                y = 1f,
                                z = 1f
                            },
                            alpha = 1f,
                            position = dynamicEntities[specialMove.Guid].position
                        });
                        outgoing.Enqueue(fakeSmoke);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Ranger:
                    #region sniper
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region cursed arrow
                        //TODO
                        #endregion
                    }
                    else {
                        #region arrow rain
                        //TODO
                        #endregion
                    }
                    break;
                #endregion
                default:
                    break;
            }
            CwRam.memory.WriteInt(CwRam.EntityStart + 0x1164, 3);//mana cubes
        }

        public static void SendUDP(byte[] data) {
            udpToServer.Send(data, data.Length);
        }
    }
}
