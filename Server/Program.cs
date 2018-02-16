using System;

namespace Server {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("server starting...");
            ServerUDP serverUDP = new ServerUDP(12346);
            Console.WriteLine("loading completed");
            Console.ReadLine();
        }
    }
}
