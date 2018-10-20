using Resources;
using Resources.Datagram;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Extensions
{
    public static class ChatMessage
    {
        public static void Init()
        {
            ServerCore.ChatMessageReceived += BroadcastMessage;
        }
        public static void Desactivate()
        {
            ServerCore.ChatMessageReceived -= BroadcastMessage;
        }
        private static void BroadcastMessage(Chat chat, Player source)
        {
            ServerCore.BroadcastUDP(chat.data, null);
        }
   
    }
}
