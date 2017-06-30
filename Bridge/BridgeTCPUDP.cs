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
        static BinaryWriter writer;
        static BinaryReader reader;

        public static void Connect(Form1 form) {
            form.Log("connecting...");
            string serverIP = form.textBoxServerIP.Text;
            int serverPort = (int)form.numericUpDownPort.Value;
            try {
                tcpToServer.Connect(serverIP, serverPort);
                form.Log($"Connected");
            } catch (IOException ex) {
                form.Log($"Connection failed: \n{ex.Message}\n");
                return; //could not connect
            }
            writer = new BinaryWriter(tcpToServer.GetStream());
            reader = new BinaryReader(tcpToServer.GetStream());
            var login = new Login() {
                name = form.textBoxUsername.Text,
                password = Hashing.Hash(form.textBoxPassword.Text)
            };
            login.Send(writer);
            switch (reader.ReadByte()) { //its not worth it to create a class for a 1byte response
                case 0: //success
                    udpToServer.Connect(serverIP, serverPort);
                    listener = new TcpListener(IPAddress.Parse("localhost"), 12345);
                    listener.Start();
                    Task.Factory.StartNew(ListenFromClientTCP);
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
            int packetID = -1;
            while (tcpToClient.Connected) {
                try {
                    packetID = reader.ReadInt32();
                } catch (IOException) {
                    break;
                }
                ProcessPacket(packetID);
            }
            //player dc
            //send dc packet to server but keep logging chat
        }
        public static void ListenFromServerTCP(Form1 form) {
            byte packetID = 255;
            while (tcpToServer.Connected) {
                try {
                    packetID = reader.ReadByte(); //we can use byte here because it doesn't contain vanilla packets
                    //player list updates
                } catch (IOException) {
                    break;
                }
                //process packet
            }
            //server offline
        }
        public static void ListenFromServerUDP() {

        }

        public static void ProcessDatagram(byte[] packet) {
            switch((Database.DatagramID)packet[0]) {
                case Database.DatagramID.entityUpdate:
                    break;
                case Database.DatagramID.hit:
                    break;
                case Database.DatagramID.shoot:
                    break;
                case Database.DatagramID.proc:
                    break;
                case Database.DatagramID.chat:
                    break;
                case Database.DatagramID.time:
                    break;
                case Database.DatagramID.interaction:
                    break;
                case Database.DatagramID.staticUpdate:
                    break;
                case Database.DatagramID.block:
                    break;
                case Database.DatagramID.particle:
                    break;
                case Database.DatagramID.connect:
                    break;
                case Database.DatagramID.disconnect:
                    break;
                case Database.DatagramID.players:
                    break;
                default:
                    break;
            }
        }
        public static void ProcessPacket(int packetID) {
            switch ((Database.PacketID)packetID) {
                case Database.PacketID.entityUpdate:
                    #region entity update
                    var entityUpdate = new EntityUpdate(reader);
                    break;
                #endregion
                case Database.PacketID.entityAction:
                    #region entity action
                    EntityAction entityAction = new EntityAction(reader);
                    switch ((Database.ActionType)entityAction.type) {
                        case Database.ActionType.talk:
                            break;

                        case Database.ActionType.staticInteraction:
                            break;

                        case Database.ActionType.pickup:
                            break;

                        case Database.ActionType.drop: //send item back to dropper because dropping is disabled to prevent chatspam
                                                        //var pickup = new Pickup() {
                                                        //    guid = player.entityData.guid,
                                                        //    item = entityAction.item
                                                        //};

                            //var serverUpdate6 = new ServerUpdate();
                            //serverUpdate6.pickups.Add(pickup);
                            //serverUpdate6.Send(player);
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
                    var hit = new Resources.Packet.Hit(reader);
                    break;
                    #endregion
                case Database.PacketID.passiveProc:
                    #region passiveProc
                    var passiveProc = new PassiveProc(reader);
                    break;
                #endregion
                case Database.PacketID.shoot:
                    #region shoot
                    var shoot = new Resources.Packet.Shoot(reader);
                    break;
                #endregion
                case Database.PacketID.chat:
                    #region chat
                    var chatMessage = new ChatMessage(reader);
                    break;
                #endregion
                case Database.PacketID.chunk:
                    #region chunk discovered
                    var chunk = new Chunk(reader);//currently not doing anything with this
                    break;
                #endregion
                case Database.PacketID.sector:
                    #region sector discovered
                    var sector = new Sector(reader);//currently not doing anything with this
                    break;
                #endregion
                case Database.PacketID.version:
                    #region version
                    var version = new ProtocolVersion();
                    break;
                #endregion
                default:
                    //unknown packet id
                    break;
            }
        }
    }
}
