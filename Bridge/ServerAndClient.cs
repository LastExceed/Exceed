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
    class ServerAndClient {
        UdpClient toServer = new UdpClient();
        TcpListener listener = new TcpListener(IPAddress.Parse("localhost"), 12345);
        TcpClient client;
        BinaryWriter writer;
        BinaryReader reader;

        public ServerAndClient() {
            toServer.Connect(IPAddress.Parse("37.24.37.78"), 12345);
        }

        public void ListenFromClient() {
            client = listener.AcceptTcpClient();
            writer = new BinaryWriter(client.GetStream());
            reader = new BinaryReader(client.GetStream());
            int packetID = -1;
            while (client.Connected) {
                try {
                    packetID = reader.ReadInt32();
                } catch (IOException) {
                    //end
                    break;
                }
                ProcessPacket(packetID);
            }
        }
        public void ProcessPacket(int packetID) {
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
                    var shoot = new Shoot(reader);
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
