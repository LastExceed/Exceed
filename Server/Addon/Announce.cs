using System;
using System.Collections.Generic;
using Resources;
using Resources.Packet;

namespace Server.Addon {
    class Announce {
        public static void Join(string current, string previous, Dictionary<ulong, Player> players) {
            if (current != previous) {
                if (previous == "") {
                    var chatMessage = new ChatMessage() {
                        message = current + " joined"
                    };
                    chatMessage.Broadcast(players, 0);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(chatMessage.message);
                    Console.ForegroundColor = ConsoleColor.White;
                } else {
                    var chatMessage = new ChatMessage() {
                        message = previous + " -> " + current
                    };
                    chatMessage.Broadcast(players, 0);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(chatMessage.message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public static void Leave(string name, Dictionary<ulong, Player> players) {
            var chatMessage = new ChatMessage() {
                message = name + " left"
            };
            chatMessage.Broadcast(players, 0);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(chatMessage.message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
