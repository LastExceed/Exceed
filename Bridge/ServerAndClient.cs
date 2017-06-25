using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bridge {
    class ServerAndClient {
        TcpListener listener = new TcpListener(IPAddress.Parse("localhost"), 12345);
        TcpClient server = new TcpClient();
        TcpClient client;
        BinaryWriter cwriter, swriter;
        BinaryReader creader, sreader;

        public ServerAndClient() {
            try {
                server.Connect("localhost", 12346);
                swriter = new BinaryWriter(server.GetStream());
                sreader = new BinaryReader(server.GetStream());
                StC();
            } catch (Exception) {
                //connection failed
            }
        }

        public void Listen() {

        }

        public void StC() {
            while (true) {
                switch (switch_on) {
                    default:
                }
            }
        }
    }
}
