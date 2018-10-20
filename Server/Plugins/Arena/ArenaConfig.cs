using System;
using System.Collections.Generic;

namespace Server.Plugins.Arena
{
    static public class ArenaConfig
    {
        public const string pluginName = "Arena";
        public static List<String> pluginDependencies = new List<String>{
            "BaseServer"
        };
    }
}
