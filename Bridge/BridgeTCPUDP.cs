using System;
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

namespace Bridge {
    static class BridgeTCPUDP {
        public static UdpClient udpToServer;
        public static TcpClient tcpToServer, tcpToClient;
        public static TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345); //hardcoded because client port can't be changed
        public static BinaryWriter swriter, cwriter;
        public static BinaryReader sreader, creader;
        public static ushort guid;
        public static int mapseed;
        public static Form1 form;
        public static bool connectedToServer = false, clientConnected = false;
        public static Dictionary<long, EntityUpdate> dynamicEntities = new Dictionary<long, EntityUpdate>();
        public static ushort lastTarget;
        public static List<Packet> outgoing = new List<Packet>();

        public static void Connect() {
            form.Log("connecting...", Color.DarkGray);
            string serverIP = form.textBoxServerIP.Text;
            int serverPort = (int)form.numericUpDownPort.Value;

            try {
                tcpToServer = new TcpClient() { NoDelay = true };
                tcpToServer.Connect(serverIP, serverPort);

                udpToServer = new UdpClient(tcpToServer.Client.LocalEndPoint as IPEndPoint);
                udpToServer.Connect(serverIP, serverPort);
            }
            catch (SocketException) {//connection refused
                Close();
                form.Log("failed\n", Color.Red);
                form.EnableButtons();
                return;
            }
            form.Log("connected\n", Color.Green);

            Stream stream = tcpToServer.GetStream();
            swriter = new BinaryWriter(stream);
            sreader = new BinaryReader(stream);

            form.Log("checking version...", Color.DarkGray);
            swriter.Write(Database.bridgeVersion);
            if (!sreader.ReadBoolean()) {
                form.Log("mismatch\n", Color.Red);
                form.buttonDisconnect.Invoke(new Action(form.buttonDisconnect.PerformClick));
                return;
            }
            form.Log("match\n", Color.Green);
            form.Log("logging in...", Color.DarkGray);
            swriter.Write(form.textBoxUsername.Text);
            swriter.Write(form.textBoxPassword.Text);
            swriter.Write(NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault());
            switch ((AuthResponse)sreader.ReadByte()) {
                case AuthResponse.success:
                    form.Log("success\n", Color.Green);
                    if (sreader.ReadBoolean()) {//if banned
                        MessageBox.Show(sreader.ReadString());//ban message
                        form.Log("you are banned\n", Color.Red);
                        goto default;
                    }
                    break;
                case AuthResponse.unknownUser:
                    form.Log("unknown username\n", Color.Red);
                    goto default;
                case AuthResponse.wrongPassword:
                    form.Log("wrong password\n", Color.Red);
                    goto default;
                default:
                    form.buttonDisconnect.Invoke(new Action(form.buttonDisconnect.PerformClick));
                    return;
            }
            guid = sreader.ReadUInt16();
            mapseed = sreader.ReadInt32();
            connectedToServer = true;
            
            swriter.Write((byte)0);//request query
            new Thread(new ThreadStart(ListenFromServerTCP)).Start();
            new Thread(new ThreadStart(ListenFromServerUDP)).Start();
            ListenFromClientTCP();
        }
        public static void Close() {
            connectedToServer = false;
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
            try {
                tcpListener.Stop();
            }
            catch { }
            dynamicEntities.Clear();
        }

        public static void ListenFromClientTCP() {
            while (connectedToServer) {
                bool WSAcancellation = false;
                try {
                    tcpListener.Start();
                    WSAcancellation = true;
                    tcpToClient = tcpListener.AcceptTcpClient();
                }
                catch (SocketException ex) {
                    if (!WSAcancellation) {
                        MessageBox.Show(ex.Message + "\n\nProbably Can't start listening for the client because the CubeWorld default port (12345) is already in use by another program. Do you have a CubeWorld server or another instance of the bridge already running on your computer?\n\nIf you don't know how to fix this, restarting your computer will likely help", "Error");
                    }
                    return;
                }
                finally {
                    tcpListener.Stop();
                }
                form.Log("client connected\n", Color.Green);

                tcpToClient.NoDelay = true;
                Stream stream = tcpToClient.GetStream();
                creader = new BinaryReader(stream);
                cwriter = new BinaryWriter(stream);

                clientConnected = true;
                new Thread(new ThreadStart(WriteToClientTCP)).Start();

                while (true) {
                    try {
                        int packetID = creader.ReadInt32();
                        ProcessClientPacket(packetID);
                    }
                    catch (IOException) {
                        clientConnected = false;
                        if (connectedToServer) {
                            SendUDP(new RemoveDynamicEntity() { Guid = (guid)}.data);
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
            while (true) {
                try {
                    ProcessServerPacket(sreader.ReadInt32()); //we can use byte here because it doesn't contain vanilla packets
                }
                catch (IOException) {
                    if (connectedToServer) {
                        form.Log("Connection to Server lost\n", Color.Red);
                        Close();
                        form.EnableButtons();
                    }
                    break;
                }
            }
        }
        public static void ListenFromServerUDP() {
            SendUDP(new byte[1] { (byte)DatagramID.dummy });//to allow incoming UDP packets
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
            while (clientConnected) {
                if (outgoing.Count != 0 && outgoing[0] != null) {
                    outgoing[0].Write(cwriter);
                    outgoing.RemoveAt(0);
                }
            }
        }

        public static void ProcessDatagram(byte[] datagram) {
            var serverUpdate = new ServerUpdate();
            bool writeServerUpdate = false;
            switch ((DatagramID)datagram[0]) {
                case DatagramID.dynamicUpdate:
                    #region entityUpdate
                    var entityUpdate = new EntityUpdate(datagram);
                    if (clientConnected) {
                        if (entityUpdate.guid == guid) {
                            CwRam.Teleport(entityUpdate.position);
                            break;
                        }
                        outgoing.Add(entityUpdate);
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
                case DatagramID.attack:
                    #region attack
                    var attack = new Attack(datagram);

                    var hit = new Hit() {
                        target = attack.Target,
                        damage = attack.Damage,
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
                case DatagramID.proc:
                    #region proc
                    var proc = new Proc(datagram);
                    if (proc.Type == ProcType.poison && proc.Target == guid) {
                        var su = new ServerUpdate();
                        su.hits.Add(new Hit() {
                            damage = proc.Modifier,
                            target = guid,
                            position = dynamicEntities[guid].position,
                        });
                        bool tick() {
                            bool f = clientConnected && dynamicEntities[guid].HP > 0;
                            if (f) {
                                outgoing.Add(su);
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
                case DatagramID.chat:
                    #region chat
                    var chat = new Chat(datagram);
                    var chatMessage = new ChatMessage() {
                        sender = chat.Sender,
                        message = chat.Text
                    };
                    if (clientConnected) outgoing.Add(chatMessage);
                    if (chat.Sender == 0) {
                        form.Log(chat.Text + "\n", Color.Magenta);
                    }
                    else {
                        form.Log(dynamicEntities[chat.Sender].name + ": ", Color.Cyan);
                        form.Log(chat.Text + "\n", Color.White);
                    }
                    break;
                #endregion
                case DatagramID.time:
                    #region time
                    var igt = new InGameTime(datagram);

                    var time = new Time() {
                        time = igt.Time
                    };
                    if (clientConnected) outgoing.Add(time);
                    break;
                #endregion
                case DatagramID.interaction:
                    #region interaction
                    var interaction = new Interaction(datagram);
                    var entityAction = new EntityAction() {
                        chunkX = interaction.ChunkX,
                        chunkY = interaction.ChunkY,
                        index = interaction.Index,
                        type = ActionType.staticInteraction
                    };
                    writeServerUpdate = true;
                    break;
                #endregion
                case DatagramID.staticUpdate:
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
                case DatagramID.block:
                    //var block = new Block(datagram);
                    //TODO
                    break;
                case DatagramID.particle:
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
                    if (clientConnected) outgoing.Add(entityUpdate);
                    dynamicEntities.Remove(rde.Guid);
                    RefreshPlayerlist();
                    break;
                #endregion
                case DatagramID.specialMove:
                    #region speicalMove
                    var specialMove = new SpecialMove(datagram);
                    switch (specialMove.Id) {
                        case SpecialMoveID.taunt:
                            if (dynamicEntities.ContainsKey(specialMove.Guid)) {
                                if (clientConnected) {
                                    CwRam.Teleport(dynamicEntities[specialMove.Guid].position);
                                    CwRam.Freeze(5000);
                                }
                            }
                            break;
                        case SpecialMoveID.cursedArrow:
                            break;
                        case SpecialMoveID.arrowRain:
                            break;
                        case SpecialMoveID.shrapnel:
                            break;
                        case SpecialMoveID.smokeBomb:
                            serverUpdate.particles.Add(new ServerUpdate.Particle() {
                                count = 1000,
                                spread = 5f,
                                type = ParticleType.noGravity,
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
                        case SpecialMoveID.iceWave:
                            break;
                        case SpecialMoveID.confusion:
                            break;
                        case SpecialMoveID.shadowStep:
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
            if (clientConnected && writeServerUpdate) outgoing.Add(serverUpdate);
        }
        public static void ProcessClientPacket(int packetID) {
            switch ((PacketID)packetID) {
                case PacketID.entityUpdate:
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
                case PacketID.entityAction:
                    #region entity action
                    EntityAction entityAction = new EntityAction(creader);
                    switch (entityAction.type) {
                        case ActionType.talk:
                            break;
                        case ActionType.staticInteraction:
                            ChatMessage x = new ChatMessage() {
                                message = "You can't use this, your hands are too small.",
                                sender = 0
                            };
                            outgoing.Add(x);
                            break;
                        case ActionType.pickup:
                            break;
                        case ActionType.drop: //send item back to dropper because dropping is disabled to prevent chatspam
                            if (form.radioButtonDestroy.Checked) {
                                outgoing.Add(new ChatMessage() {
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
                                outgoing.Add(serverUpdate);
                            }
                            break;
                        case ActionType.callPet:
                            break;
                        default:
                            //unknown type
                            break;
                    }
                    break;
                #endregion
                case PacketID.hit:
                    #region hit
                    var hit = new Hit(creader);
                    var attack = new Attack() {
                        Target = (ushort)hit.target,
                        Damage = hit.damage,
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
                case PacketID.passiveProc:
                    #region passiveProc
                    var passiveProc = new PassiveProc(creader);
                    switch (passiveProc.type) {
                        case ProcType.bulwalk:
                            outgoing.Add(new ChatMessage() {
                                message = string.Format("bulwalk: {0}% dmg reduction", 1.0f - passiveProc.modifier),
                                sender = 0,
                            });
                            break;
                        case ProcType.warFrenzy:
                            break;
                        case ProcType.camouflage:
                            break;
                        case ProcType.poison:
                            break;
                        case ProcType.unknownA:
                            break;
                        case ProcType.manashield:
                            outgoing.Add(new ChatMessage() {
                                message = string.Format("manashield: {0}", passiveProc.modifier),
                                sender = 0,
                            });
                            break;
                        case ProcType.unknownB:
                            break;
                        case ProcType.unknownC:
                            break;
                        case ProcType.fireSpark:
                            break;
                        case ProcType.intuition:
                            break;
                        case ProcType.elusivenes:
                            break;
                        case ProcType.swiftness:
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
                case PacketID.shoot:
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
                case PacketID.chat:
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

                        outgoing.Add(serverUpdate);
                    }
                    else {
                        var chat = new Chat(chatMessage.message) {
                            Sender = guid//client doesn't send this //(ushort)chatMessage.sender
                        };
                        SendUDP(chat.data);
                    }
                    break;
                #endregion
                case PacketID.chunk:
                    #region chunk
                    var chunk = new Chunk(creader);
                    break;
                #endregion
                case PacketID.sector:
                    #region sector
                    var sector = new Sector(creader);
                    break;
                #endregion
                case PacketID.version:
                    #region version
                    var version = new ProtocolVersion(creader);
                    if (version.version != 3) {
                        version.version = 3;
                        outgoing.Add(version);
                    }
                    else {
                        outgoing.Add(new Join() {
                            guid = guid,
                            junk = new byte[0x1168]
                        });
                        //outgoing.Add(new MapSeed() {
                        //    seed = mapseed
                        //});
                        foreach (var dynamicEntity in dynamicEntities.Values.ToList()) {
                            outgoing.Add(dynamicEntity);
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
                    var query = new Query(sreader);
                    foreach (var item in query.players) {
                        if (!dynamicEntities.ContainsKey(item.Key)) {
                            dynamicEntities.Add(item.Key, new EntityUpdate());
                        }
                        dynamicEntities[item.Key].guid = item.Key;
                        dynamicEntities[item.Key].name = item.Value;
                    }
                    form.Invoke(new Action(form.listBoxPlayers.Items.Clear));
                    foreach (var playerData in dynamicEntities.Values) {
                        form.Invoke(new Action(() => form.listBoxPlayers.Items.Add(playerData.name)));
                    }
                    break;
                case 4:
                    outgoing.Add(new ServerUpdate(sreader));
                    break;
                default:
                    MessageBox.Show("unknown server packet received");
                    break;
            }
        }

        public static void RefreshPlayerlist() {
            form.Invoke((Action)form.listBoxPlayers.Items.Clear);
            foreach (var dynamicEntity in dynamicEntities.Values.ToList()) {
                if (dynamicEntity.hostility == Hostility.player) {
                    form.Invoke(new Action(() => form.listBoxPlayers.Items.Add(dynamicEntity.name)));
                }
            }
        }

        public static void OnHotkey(int hotkeyID) {
            HotkeyID hotkey = (HotkeyID)hotkeyID;
            if (hotkey == HotkeyID.teleport_to_town) {
                CwRam.SetMode(Mode.teleport_to_city, 0);
                return;
            }

            bool spec = dynamicEntities[guid].specialization == 1;
            bool space = hotkeyID == 1;
            switch ((EntityClass)dynamicEntities[guid].entityClass) {
                case EntityClass.Rogue when spec:
                    #region ninja
                    if (hotkey == HotkeyID.ctrlSpace) {
                        #region dash
                        CwRam.SetMode(Mode.spin_run, 0);
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
                    if (hotkey == HotkeyID.ctrlSpace) {
                        #region confusion
                        var specialMove = new SpecialMove() {
                            Guid = guid,
                            Id = SpecialMoveID.confusion,
                        };
                        SendUDP(specialMove.data);
                        #endregion
                    }
                    else {
                        #region shadow step
                        var specialMove = new SpecialMove() {
                            Guid = guid,
                            Id = SpecialMoveID.shadowStep,
                        };
                        SendUDP(specialMove.data);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Warrior when spec:
                    #region guardian
                    if (hotkey == HotkeyID.ctrlSpace) {
                        #region taunt
                        var specialMove = new SpecialMove() {
                            Guid = lastTarget,
                            Id = SpecialMoveID.taunt,
                        };
                        SendUDP(specialMove.data);
                        #endregion
                    }
                    else {
                        #region steel wall
                        CwRam.SetMode(Mode.boss_skill_block, 0);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Warrior:
                    #region berserk
                    if (hotkey == HotkeyID.ctrlSpace) {
                        #region boulder toss
                        CwRam.SetMode(Mode.boulder_toss, 0);
                        #endregion
                    }
                    else {
                        #region earth shatter
                        CwRam.SetMode(Mode.earth_shatter, 0);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Mage when spec:
                    #region watermage
                    if (hotkey == HotkeyID.ctrlSpace) {
                        #region splash
                        CwRam.SetMode(Mode.splash, 0);
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
                    if (hotkey == HotkeyID.ctrlSpace) {
                        #region lava
                        CwRam.SetMode(Mode.lava, 0);
                        #endregion
                    }
                    else {
                        #region beam
                        CwRam.SetMode(Mode.fireray, 0);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Ranger when spec:
                    #region scout
                    if (hotkey == HotkeyID.ctrlSpace) {
                        #region shrapnel
                        //TODO
                        #endregion
                    }
                    else {
                        #region smoke bomb
                        var specialMove = new SpecialMove() {
                            Guid = guid,
                            Id = SpecialMoveID.smokeBomb,
                        };
                        SendUDP(specialMove.data);

                        var fakeSmoke = new ServerUpdate();
                        fakeSmoke.particles.Add(new ServerUpdate.Particle() {
                            count = 1000,
                            spread = 5f,
                            type = ParticleType.noGravity,
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
                        outgoing.Add(fakeSmoke);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Ranger:
                    #region sniper
                    if (hotkey == HotkeyID.ctrlSpace) {
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
