using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Server {
    class ServerUDP {
        UdpClient UDPclient = new UdpClient(new IPEndPoint(IPAddress.Any, 12345));
        IPEndPoint source;

        public void listen() {
            while (true) {
                byte[] data = UDPclient.Receive(ref source);
                process_packet(data);
            }
        }

        public void process_packet(byte[] data) {
            switch (data[0]) {
                case 0:
                    break;

                default:
                    break;
            }
        }
    }
}
