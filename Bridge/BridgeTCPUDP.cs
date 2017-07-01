using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using Resources;
using Resources.Packet;
using Resources.Packet.Part;
using Resources.Datagram;

namespace Bridge {
    static class BridgeTCPUDP {
        static UdpClient udpToServer = new UdpClient();
        static TcpClient tcpToServer = new TcpClient();
        static TcpClient tcpToClient;
        static TcpListener listener;
        static BinaryWriter swriter, cwriter;
        static BinaryReader sreader, creader;
        static ushort guid;

        public static void Connect(Form1 form) {
            form.Log("connecting...");
            string serverIP = form.textBoxServerIP.Text;
            int serverPort = (int)form.numericUpDownPort.Value;
            try {
                tcpToServer.Connect(serverIP, serverPort);
                form.Log($"Connected");
            } catch(IOException ex) {
                form.Log($"Connection failed: \n{ex.Message}\n");
                return; //could not connect
            }
            swriter = new BinaryWriter(tcpToServer.GetStream());
            sreader = new BinaryReader(tcpToServer.GetStream());

            #region secure login transfer
            string publicKey = sreader.ReadString();

            var username = Hashing.Encrypt(publicKey, form.textBoxUsername.Text);
            swriter.Write(username.Length);
            swriter.Write(username); //Send username

            var salt = sreader.ReadBytes(16); // get salt

            var hash = Hashing.Encrypt(publicKey, Hashing.Hash(form.textBoxPassword.Text, salt));
            swriter.Write(hash.Length);
            swriter.Write(hash); //send hashed password
            #endregion

            switch(sreader.ReadByte()) {
                case 0: //success
                    udpToServer.Connect(serverIP, serverPort);
                    listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
                    listener.Start();
                    Task.Factory.StartNew(ListenFromClientTCP);
                    Task.Factory.StartNew(ListenFromServerUDP);
                    ListenFromServerTCP(form);
                    break;
                case 1: //failed
                case 2: //banned
                default://unknown response
                    tcpToServer.Close();
                    break;
            }
        }

        public static void Stop() {
            listener.Stop();
            tcpToClient.Close();
            udpToServer.Close();
            tcpToServer.Close();
        }

        public static void ListenFromClientTCP() {
            tcpToClient = listener.AcceptTcpClient();
            listener.Stop();
            creader = new BinaryReader(tcpToClient.GetStream());
            cwriter = new BinaryWriter(tcpToClient.GetStream());
            int packetID = -1;
            while(tcpToClient.Connected) {
                try {
                    packetID = creader.ReadInt32();
                } catch (IOException) {
                    break;
                }
                ProcessClientPacket(packetID);
            }
            var dc = new Disconnect() {
                Guid = guid
            };
            SendUDP(dc.data);
        }

        public static void ListenFromServerTCP(Form1 form) {
            byte packetID = 255;
            while(tcpToServer.Connected) {
                try {
                    packetID = sreader.ReadByte(); //we can use byte here because it doesn't contain vanilla packets
                    //player list updates
                } catch(IOException) {
                    break;
                }
                ProcessServerPacket(packetID);
            }
            tcpToClient.Close();
        }
        public static void ListenFromServerUDP() {
            IPEndPoint source = null;
            while(true) {
                byte[] datagram = udpToServer.Receive(ref source);
                if(true) { //source == server
                    ProcessDatagram(datagram); //might require try n' catch here, we'll see
                }
            }
        }

        public static void ProcessDatagram(byte[] datagram) {
            switch((Database.DatagramID)datagram[0]) {
                case Database.DatagramID.entityUpdate:
                    var update = new EntityUpdate(new BinaryReader(new MemoryStream(datagram)));
                    update.Write(cwriter, true);
                    break;
                case Database.DatagramID.attack:
                    var attack = new Attack(datagram);

                    var hit = new Hit() {
                        target = attack.Target,
                        damage = attack.Damage,
                        critical = attack.Critical ? 1 : 0,
                        stuntime = attack.Stuntime,
                        //hit.position = attack.
                        skill = attack.Skill,
                        type = (byte)attack.Type,
                        showlight = (byte)(attack.ShowLight ? 1 : 0)
                    };
                    hit.Write(cwriter, true);
                    break;
                case Database.DatagramID.shoot:
                    var shootDatagram = new Resources.Datagram.Shoot(datagram);

                    var shootPacket = new Resources.Packet.Shoot() {
                        position = shootDatagram.Position,
                        velocity = shootDatagram.Velocity,
                        scale = shootDatagram.Scale,
                        particles = shootDatagram.Particles,
                        projectile = (int)shootDatagram.Projectile
                    };
                    shootPacket.Write(cwriter, true);
                    break;
                case Database.DatagramID.proc:
                    var proc = new Proc(datagram);

                    var passiveProc = new PassiveProc() {
                        target = proc.Target,
                        type = (byte)proc.Type,
                        modifier = proc.Modifier,
                        duration = proc.Duration
                    };
                    passiveProc.Write(cwriter, true);
                    break;
                case Database.DatagramID.chat:
                    var chat = new Chat(datagram);

                    var chatMessage = new ChatMessage() {
                        sender = chat.Sender,
                        message = chat.Text
                    };
                    chatMessage.Write(cwriter, true);
                    break;
                case Database.DatagramID.time:
                    var igt = new InGameTime(datagram);

                    var time = new Time() {
                        time = igt.Time
                    };
                    time.Write(cwriter, true);
                    break;
                case Database.DatagramID.interaction:
                    var interaction = new Interaction(datagram);

                    var entityAction = new EntityAction() {
                        chunkX = interaction.ChunkX,
                        chunkY = interaction.ChunkY,
                        index = interaction.Index,
                        type = (byte)Database.ActionType.staticInteraction
                    };
                    entityAction.Write(cwriter, true);
                    break;
                case Database.DatagramID.staticUpdate:
                    var staticUpdate = new StaticUpdate(datagram);

                    var staticEntity = new StaticEntity() {
                        chunkX = (int)(staticUpdate.Position.x / (65536 * 256)),
                        chunkY = (int)(staticUpdate.Position.y / (65536 * 256)),
                        id = staticUpdate.Id,
                        type = (int)staticUpdate.Type,
                        position = staticUpdate.Position,
                        rotation = (int)staticUpdate.Direction,
                        size = staticUpdate.Size,
                        closed = staticUpdate.Closed ? 1:0,
                        time = staticUpdate.Time,
                        guid = staticUpdate.User
                    };
                    var staticServerUpdate = new ServerUpdate();
                    staticServerUpdate.statics.Add(staticEntity);
                    staticServerUpdate.Write(cwriter, true);
                    break;
                case Database.DatagramID.block:
                    //var block = new Block(datagram);
                    //TODO
                    break;
                case Database.DatagramID.particle:
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

                    var particleServerUpdate = new ServerUpdate();
                    particleServerUpdate.particles.Add(particleSubPacket);
                    particleServerUpdate.Write(cwriter, true);
                    break;
                case Database.DatagramID.connect:
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
                case Database.DatagramID.disconnect:
                    break;
                case Database.DatagramID.players:
                    break;
                default:
                    break;
            }
        }

        public static void ProcessClientPacket(int packetID) {
            switch((Database.PacketID)packetID) {
                case Database.PacketID.entityUpdate:
                    break;
                case Database.PacketID.multiEntityUpdate:
                    break;
                case Database.PacketID.entityUpdatesFinished:
                    break;
                case Database.PacketID.unknown3:
                    break;
                case Database.PacketID.serverUpdate:
                    break;
                case Database.PacketID.time:
                    break;
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
                            var pickup = new Pickup() {
                                guid = guid,
                                item = entityAction.item
                            };

                            var serverUpdate = new ServerUpdate();
                            serverUpdate.pickups.Add(pickup);
                            serverUpdate.Write(cwriter, true);
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
                        Sender = (ushort)chatMessage.sender
                    };
                    SendUDP(chat.data);
                    break;
                    #endregion
                case Database.PacketID.chunk:
                    break;
                case Database.PacketID.sector:
                    break;
                case Database.PacketID.unknown13:
                    break;
                case Database.PacketID.unknown14:
                    break;
                case Database.PacketID.mapseed:
                    break;
                case Database.PacketID.joinPacket:
                    break;
                case Database.PacketID.version:
                    #region version
                    var version = new ProtocolVersion(creader);
                    if(version.version != 3) {
                        version.version = 3;
                        version.Write(cwriter, true);
                    } else {
                        SendUDP(new Connect().data);
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
