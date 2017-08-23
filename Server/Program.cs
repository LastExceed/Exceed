using System;

namespace Server {
    class Program {
        static void Main(string[] args) {
            //ServerTCP TCPserver = new ServerTCP(12345);
            ServerUDP serverUDP = new ServerUDP(12346);
            Console.ReadLine();
        }
    }
}
