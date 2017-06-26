using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Resources;
using Resources.Datagram;

namespace Server {
    class ServerUDP {
        UdpClient UDPclient;
        Dictionary<ushort, IPEndPoint> players;

        public ServerUDP() {
            UDPclient = new UdpClient(new IPEndPoint(IPAddress.Any, 12345));
            players = new Dictionary<ushort, IPEndPoint>();
        }

        public void Listen() {
            IPEndPoint source = null;
            while(true) {
                byte[] data = UDPclient.Receive(ref source);
                ProcessPacket(data, source);
            }
        }

        public void SendToAll(byte[] data, IPEndPoint source) {
            foreach(var item in players) {
                if(item.Value != source)
                    UDPclient.Send(data, data.Length, item.Value);
            }
        }

        public void ProcessPacket(byte[] packet, IPEndPoint source) {
            var type = (Database.DatagramID)packet[0];
            switch(type) {
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
                    var d = new Disconnect(packet);
                    players.Remove(d.Guid);
                    break;
                case Database.DatagramID.players:
                    break;
                default:
                    Console.WriteLine("unknown packet ID: " + type); //causes some console spam, but allows resyncing with the player without DC or crash
                    break;
            }
        }
    }
}
