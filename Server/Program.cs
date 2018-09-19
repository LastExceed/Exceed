using System;

namespace Server {
    class Program {
        static void Main(string[] args) {
            Server.Start(12346);
            while (true) {
                Console.ReadLine();
            }
        }
    }
}
