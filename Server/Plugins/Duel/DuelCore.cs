using System;
using System.Collections.Generic;
using Resources;

namespace Server.Plugins.Duel
{
    class DuelCore : PluginBase
    {
        public volatile List<DuelSystem> duels = new List<DuelSystem>();
        public List<PlayerDuel> players = new List<PlayerDuel>();
        CommandsBase command;
        public DuelCore()
        {
            command = new Commands(this);
            pluginName = "Duel";
        }
        public override Boolean hasCommands()
        {
            return true;
        }
        public override Boolean analyzeCommand(string message, Player source)
        {
            return command.ParseAsCommand(message, source);
        }
        public override List<string> checkDependencies()
        {
            return DuelConfig.pluginDependencies;
        }
    }
}
