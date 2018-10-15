using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Extensions {
    public static class Extensions {
        public static void Init() {
            AntiCheat.Init();
            Pvp.Init();
            ChatManager.Init();
            SpecialMoves.Init();
            Balancing.Init();
        }
    }
}
