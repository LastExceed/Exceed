using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Resources;
using Server.Plugins.Arena.Database;
using Server.Plugins.Arena.Resources;

namespace Server.Plugins.Arena
{
    class ArenaCore : PluginBase
    {
        public ArenaDatabase ArenaDatabase;
        CommandsBase command;
        public ArenaCore()
        {
            ArenaDatabase = new ArenaDatabase();
            ArenaDatabase.Database.Migrate();
            command = new Commands(this);
            pluginName = ArenaConfig.pluginName;
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
            return ArenaConfig.pluginDependencies;
        }
    }
}
