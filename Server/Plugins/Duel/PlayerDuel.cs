using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Resources;

namespace Server.Plugins.Duel
{
    class PlayerDuel : Player
    {
        public bool? Duel;
        public bool? PreparingTime;
        public PlayerDuel(TcpClient tcpClient) : base(tcpClient)
        {
        }
    }
}
