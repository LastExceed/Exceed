﻿using System;

namespace Server {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("server starting...");
            ServerUDP.Setup(12346);
            Console.WriteLine("loading completed");
            while (Console.ReadLine() != "stop");
        }
    }
}
