using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins.Duel
{
    class DuelCore : PluginBase
    {
        public static volatile List<DuelSystem> duels = new List<DuelSystem>();
        public static List<PlayerDuel> players = new List<PlayerDuel>();
        public DuelCore()
        {
            Commands.Init(this);
            pluginName = "Duel";
        }
        public override Boolean hasCommands()
        {
            return true;
        }
    }
}
