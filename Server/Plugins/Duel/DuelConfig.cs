using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins.Duel
{
    static public class DuelConfig
    {
        public const string pluginName = "Duel";
        public static List<String> pluginDependencies = new List<String>{
            "BaseServer",
            "Arena"
        };
        public const int maxRequestTime = 30;
    }
}
