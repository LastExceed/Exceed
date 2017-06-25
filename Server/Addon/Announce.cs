using System;
using System.Collections.Generic;
using Resources;
using Resources.Packet;

namespace Server.Addon {
    class Announce {
        public static void join(string current, string previous, Dictionary<ulong, Player> players) {
            if (current != previous) {
                if (previous == "") {
                    var chatMessage = new ChatMessage();
                    chatMessage.message = current + " joined";
                    chatMessage.send(players, 0);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(chatMessage.message);
                    Console.ForegroundColor = ConsoleColor.White;
                } else {
                    var chatMessage = new ChatMessage();
                    chatMessage.message = previous + " -> " + current;
                    chatMessage.send(players, 0);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(chatMessage.message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public static void leave(string name, Dictionary<ulong, Player> players) {
            var chatMessage = new ChatMessage();
            chatMessage.message = name + " left";
            chatMessage.send(players, 0);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(chatMessage.message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
