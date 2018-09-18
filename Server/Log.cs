using System;
using System.Collections.Generic;
using System.Text;

namespace Server {
    static class Log {
        const ConsoleColor defaultColor = ConsoleColor.White;

        public static void Print(string text, ConsoleColor color = defaultColor, bool timeStamp = true) {
            if (timeStamp) {
                var time = DateTime.Now.ToLongTimeString();
                Console.Write($"[{time}] ");
            }
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = defaultColor;
        }

        public static void PrintLn(string text, ConsoleColor color = defaultColor, bool timeStamp = true) {
            Print(text + Environment.NewLine, color, timeStamp);
        }
    }
}
