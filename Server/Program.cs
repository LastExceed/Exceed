using Server.Extensions;
using System;

namespace Server {
    class Program {
        static void Main(string[] args) {
            ServerCore.Start(12346);
            while (true) {

                ConsoleCommands.analyzeCommand(Console.ReadLine());
            }
        }
    }
}
