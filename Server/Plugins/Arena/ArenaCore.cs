using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Server.Plugins.Arena.Database;

namespace Server.Plugins.Arena
{
    class ArenaCore : PluginBase
    {
        public static ArenaDatabase ArenaDatabase;
        public ArenaCore()
        {
            ArenaDatabase = new ArenaDatabase();
            ArenaDatabase.Database.Migrate();
            Commands.Init(ArenaDatabase,this);
            pluginName = ArenaConfig.pluginName;
        }
        public override Boolean hasCommands()
        {
            return true;
        }
    }
}
