using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Resources;
using Resources.Packet;
using Resources.Packet.Part;
using Resources.Datagram;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Generic;
using ReadWriteProcessMemory;

namespace Bridge {
    static class BridgeTCPUDP {
        public static UdpClient udpToServer;
        public static TcpClient tcpToServer, tcpToClient;
        public static TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345); //hardcoded because client port can't be changed
        public static BinaryWriter swriter, cwriter;
        public static BinaryReader sreader, creader;
        public static ushort guid;
        public static Form1 form;
        public static bool connected = false;
        public static Dictionary<ulong, EntityUpdate> players = new Dictionary<ulong, EntityUpdate>();

        public static void Connect() {
            form.Log("connecting...", Color.DarkGray);
            string serverIP = form.textBoxServerIP.Text;
            int serverPort = (int)form.numericUpDownPort.Value;

            try {
                tcpToServer = new TcpClient() { NoDelay = true };
                tcpToServer.Connect(serverIP, serverPort);

                udpToServer = new UdpClient(tcpToServer.Client.LocalEndPoint as IPEndPoint);
                udpToServer.Connect(serverIP, serverPort);
                form.Log("connected\n", Color.Green);
            } catch(Exception ex) {
                tcpToServer.Close();
                tcpToServer = null;

                udpToServer.Close();
                udpToServer = null;

                form.Log($"Connection failed: \n{ex.Message}\n", Color.Red);
                form.EnableButtons();
                return;
            }
            
            Stream stream = tcpToServer.GetStream();
            swriter = new BinaryWriter(stream);
            sreader = new BinaryReader(stream);
            form.Log("authenticating...", Color.DarkGray);

            #region secure login transfer            
            /*
            swriter.Write(form.textBoxUsername.Text); //Send username
            swriter.Write(Hashing.Hash(form.textBoxPassword.Text, sreader.ReadBytes(Hashing.saltSize))); //send hashed password
            */
            #endregion

            swriter.Write(123);

            switch((Database.LoginResponse)sreader.ReadByte()) {
                case Database.LoginResponse.success:
                    form.Log("success\n", Color.Green);
                    connected = true;
                    Task.Factory.StartNew(ListenFromClientTCP);
                    Task.Factory.StartNew(ListenFromServerTCP);
                    Task.Factory.StartNew(ListenFromServerUDP);
                    break;
                case Database.LoginResponse.fail:
                    MessageBox.Show("Wrong Username/Password");
                    goto default;
                case Database.LoginResponse.banned:
                    MessageBox.Show("You are banned");
                    goto default;
                default:
                    form.Log("failed\n", Color.Red);
                    form.buttonDisconnect.Invoke(new Action(form.buttonDisconnect.PerformClick));
                    break;
            }
        }
        public static void Close() {
            connected = false;
            LingerOption lingerOption = new LingerOption(true, 0);
            try {
                udpToServer.Close();
            } catch { }
            try {
                tcpToServer.LingerState = lingerOption;
                tcpToServer.Client.Close();
                tcpToServer.Close();
            } catch { }
            try {
                tcpToClient.LingerState = lingerOption;
                tcpToClient.Client.Close();
                tcpToClient.Close();
            } catch { }
            try {
                tcpListener.Stop();
            } catch { }
        }

        public static void ListenFromClientTCP() {
            while (connected) {
                tcpListener.Start();
                try {
                    tcpToClient = tcpListener.AcceptTcpClient();
                } catch (SocketException) {
                    //WSAcancellationblabla
                    return;
                }
                tcpListener.Stop();
                CwRam.memory = new ProcessMemory("Cube");
                form.Invoke(new Action(() => form.panelItemEditor.Enabled = true));

                form.Log("client connected\n", Color.Green);
                tcpToClient.NoDelay = true;
                creader = new BinaryReader(tcpToClient.GetStream());
                cwriter = new BinaryWriter(tcpToClient.GetStream());
                int packetID;
                while (true) {
                    try {
                        packetID = creader.ReadInt32();
                        ProcessClientPacket(packetID);
                    } catch (IOException) {
                        if (connected) {
                            SendUDP(new Disconnect() { Guid = guid }.data);
                        }
                        break;
                    } catch (ObjectDisposedException) { }
                }
                form.Log("client disconnected\n", Color.Red);
            }
        }
        public static void ListenFromServerTCP() {
            while(true) {
                try {
                    ProcessServerPacket(sreader.ReadInt32()); //we can use byte here because it doesn't contain vanilla packets
                } catch(IOException) {
                    if (connected) {
                        form.Log("Connection to Server lost\n", Color.Red);
                        Close();
                        form.EnableButtons();
                    }
                    break;
                }
            }
        }
        public static void ListenFromServerUDP() {
            IPEndPoint source = null;
            try {
                while (true) {
                    byte[] datagram = udpToServer.Receive(ref source);
                    try {
                        ProcessDatagram(datagram);
                    } catch (IOException) {
                        return;
                    }
                }
            } catch (SocketException) {
                //when UDPclient is closed
            } catch (IOException) {
                //when bridge tries to pass a packet to
                //the client while the client disconnects
            }
        }

        public static void ProcessDatagram(byte[] datagram) {
            var serverUpdate = new ServerUpdate();
            switch ((Database.DatagramID)datagram[0]) {
                case Database.DatagramID.entityUpdate:
                    #region entityUpdate
                    var entityUpdate = new EntityUpdate(datagram);

                    if (entityUpdate.guid == guid) {
                        CwRam.memory.WriteLong(CwRam.EntityStart + 0x10, entityUpdate.position.x);
                        CwRam.memory.WriteLong(CwRam.EntityStart + 0x18, entityUpdate.position.y);
                        CwRam.memory.WriteLong(CwRam.EntityStart + 0x20, entityUpdate.position.z);
                    } else {
                        entityUpdate.Write(cwriter);
                    }

                    if (players.ContainsKey(entityUpdate.guid)) {
                        entityUpdate.Merge(players[entityUpdate.guid]);
                    } else {
                        players.Add(entityUpdate.guid, entityUpdate);
                    }
                    break;
                #endregion
                case Database.DatagramID.attack:
                    #region attack
                    var attack = new Attack(datagram);

                    var hit = new Hit() {
                        target = attack.Target,
                        damage = attack.Damage,
                        critical = attack.Critical ? 1 : 0,
                        stuntime = attack.Stuntime,
                        position = players[attack.Target].position,
                        skill = attack.Skill,
                        type = (byte)attack.Type,
                        showlight = (byte)(attack.ShowLight ? 1 : 0)
                    };
                    serverUpdate.hits.Add(hit);
                    serverUpdate.Write(cwriter);
                    break;
                #endregion
                case Database.DatagramID.shoot:
                    #region shoot
                    var shootDatagram = new Resources.Datagram.Shoot(datagram);

                    var shootPacket = new Resources.Packet.Shoot() {
                        position = shootDatagram.Position,
                        velocity = shootDatagram.Velocity,
                        scale = shootDatagram.Scale,
                        particles = shootDatagram.Particles,
                        projectile = (int)shootDatagram.Projectile
                    };
                    serverUpdate.shoots.Add(shootPacket);
                    serverUpdate.Write(cwriter);
                    break;
                #endregion
                case Database.DatagramID.proc:
                    #region proc
                    var proc = new Proc(datagram);

                    var passiveProc = new PassiveProc() {
                        target = proc.Target,
                        type = (byte)proc.Type,
                        modifier = proc.Modifier,
                        duration = proc.Duration
                    };
                    serverUpdate.passiveProcs.Add(passiveProc);
                    serverUpdate.Write(cwriter);
                    break;
                #endregion
                case Database.DatagramID.chat:
                    #region chat
                    var chat = new Chat(datagram);

                    var chatMessage = new ChatMessage() {
                        sender = chat.Sender,
                        message = chat.Text
                    };
                    chatMessage.Write(cwriter, true);
                    form.Log(chat.Sender + ": ", Color.Cyan);
                    form.Log(chat.Text + "\n", Color.White);
                    break;
                #endregion
                case Database.DatagramID.time:
                    #region time
                    var igt = new InGameTime(datagram);

                    var time = new Time() {
                        time = igt.Time
                    };
                    time.Write(cwriter);
                    break;
                #endregion
                case Database.DatagramID.interaction:
                    #region interaction
                    var interaction = new Interaction(datagram);

                    var entityAction = new EntityAction() {
                        chunkX = interaction.ChunkX,
                        chunkY = interaction.ChunkY,
                        index = interaction.Index,
                        type = (byte)Database.ActionType.staticInteraction
                    };
                    //serverUpdate..Add();
                    //serverUpdate.Write(cwriter);
                    break;
                #endregion
                case Database.DatagramID.staticUpdate:
                    #region staticUpdate
                    var staticUpdate = new StaticUpdate(datagram);

                    var staticEntity = new StaticEntity() {
                        chunkX = (int)(staticUpdate.Position.x / (65536 * 256)),
                        chunkY = (int)(staticUpdate.Position.y / (65536 * 256)),
                        id = staticUpdate.Id,
                        type = (int)staticUpdate.Type,
                        position = staticUpdate.Position,
                        rotation = (int)staticUpdate.Direction,
                        size = staticUpdate.Size,
                        closed = staticUpdate.Closed ? 1 : 0,
                        time = staticUpdate.Time,
                        guid = staticUpdate.User
                    };
                    var staticServerUpdate = new ServerUpdate();
                    staticServerUpdate.statics.Add(staticEntity);
                    staticServerUpdate.Write(cwriter, true);
                    break;
                #endregion
                case Database.DatagramID.block:
                    //var block = new Block(datagram);
                    //TODO
                    break;
                case Database.DatagramID.particle:
                    #region particle
                    var particleDatagram = new Resources.Datagram.Particle(datagram);

                    var particleSubPacket = new Resources.Packet.Part.Particle() {
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
                        type = (int)particleDatagram.Type,
                        spread = particleDatagram.Spread
                    };
                    serverUpdate.particles.Add(particleSubPacket);
                    serverUpdate.Write(cwriter, true);
                    break;
                #endregion
                case Database.DatagramID.connect:
                    #region connect
                    var connect = new Connect(datagram);
                    guid = connect.Guid;

                    var join = new Join() {
                        guid = guid,
                        junk = new byte[0x1168]
                    };
                    join.Write(cwriter, true);

                    var mapseed = new MapSeed() {
                        seed = connect.Mapseed
                    };
                    mapseed.Write(cwriter, true);
                    break;
                #endregion
                case Database.DatagramID.disconnect:
                    #region disconnect
                    var disconnect = new Disconnect(datagram);
                    var pdc = new Resources.Packet.EntityUpdate() {
                        guid = disconnect.Guid,
                        hostility = 255, //workaround for DC because i dont like packet2
                        HP = 0
                    };
                    pdc.Write(cwriter);
                    break;
                #endregion
                default:
                    break;
            }
        }
        public static void ProcessClientPacket(int packetID) {
            switch((Database.PacketID)packetID) {
                case Database.PacketID.entityUpdate:
                    #region entityUpdate
                    var entityUpdate = new EntityUpdate(creader);
                    if (players.ContainsKey(entityUpdate.guid)) {
                        entityUpdate.Merge(players[entityUpdate.guid]);
                    } else {
                        players.Add(entityUpdate.guid, entityUpdate);
                    }
                    SendUDP(entityUpdate.Data);
                    break;
                #endregion
                case Database.PacketID.entityAction:
                    #region entity action
                    EntityAction entityAction = new EntityAction(creader);
                    switch((Database.ActionType)entityAction.type) {
                        case Database.ActionType.talk:
                            break;
                        case Database.ActionType.staticInteraction:
                            break;
                        case Database.ActionType.pickup:
                            break;
                        case Database.ActionType.drop: //send item back to dropper because dropping is disabled to prevent chatspam
                            var serverUpdate = new ServerUpdate();
                            serverUpdate.pickups.Add(new Pickup() { guid = guid, item = entityAction.item });
                            serverUpdate.Write(cwriter);
                            break;
                        case Database.ActionType.callPet:
                            break;
                        default:
                            //unknown type
                            break;
                    }
                    break;
                #endregion
                case Database.PacketID.hit:
                    #region hit
                    var hit = new Hit(creader);

                    var attack = new Attack() {
                        Target = (ushort)hit.target,
                        Damage = hit.damage,
                        Stuntime = hit.stuntime,
                        Skill = hit.skill,
                        Type = (Database.DamageType)hit.type,
                        ShowLight = hit.showlight == 1,
                        Critical = hit.critical == 1
                    };
                    SendUDP(attack.data);
                    break;
                #endregion
                case Database.PacketID.passiveProc:
                    #region passiveProc
                    var passiveProc = new PassiveProc(creader);

                    var proc = new Proc() {
                        Target = (ushort)passiveProc.target,
                        Type = (Database.ProcType)passiveProc.type,
                        Modifier = passiveProc.modifier,
                        Duration = passiveProc.duration
                    };
                    SendUDP(proc.data);

                    break;
                #endregion
                case Database.PacketID.shoot:
                    #region shoot
                    var shootPacket = new Resources.Packet.Shoot(creader);

                    var shootDatagram = new Resources.Datagram.Shoot() {
                        Position = shootPacket.position,
                        Velocity = shootPacket.velocity,
                        Scale = shootPacket.scale,
                        Particles = shootPacket.particles,
                        Projectile = (Database.Projectile)shootPacket.projectile
                    };
                    SendUDP(shootDatagram.data);
                    break;
                #endregion
                case Database.PacketID.chat:
                    #region chat
                    var chatMessage = new ChatMessage(creader);

                    var chat = new Chat(chatMessage.message) {
                        Sender = guid//client doesn't send this (ushort)chatMessage.sender
                    };
                    SendUDP(chat.data);
                    break;
                #endregion
                case Database.PacketID.chunk:
                    #region chunk
                    var chunk = new Chunk(creader);
                    break;
                #endregion
                case Database.PacketID.sector:
                    #region sector
                    var sector = new Sector(creader);
                    break;
                #endregion
                case Database.PacketID.version:
                    #region version
                    var version = new ProtocolVersion(creader);
                    if(version.version != 3) {
                        version.version = 3;
                        version.Write(cwriter, true);
                    } else {
                        var connect = new Connect();
                        SendUDP(connect.data);
                    }
                    break;
                #endregion
                case Database.PacketID.serverFull:
                    break;
                default:
                    //unknown packet id
                    break;
            }
        }
        public static void ProcessServerPacket(int packetID) {
            switch(packetID) {
                case 4:
                    int count = sreader.ReadInt32();
                    byte[] buffer = sreader.ReadBytes(count);
                    cwriter.Write(4);
                    cwriter.Write(count);
                    cwriter.Write(buffer);
                    break;
                default:
                    //Unknown package
                    break;
            }
        }

        public static void SendUDP(byte[] data) {
            udpToServer.Send(data, data.Length);
        }
    }
}
