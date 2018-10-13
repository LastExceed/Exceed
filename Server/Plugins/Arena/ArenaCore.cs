using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Server.Plugins.Arena.Database;

namespace Server.Plugins.Arena
{
    class ArenaCore : Plugin
    {
        public static ArenaDatabase ArenaDatabase;
        public ArenaCore()
        {
            ArenaDatabase = new ArenaDatabase();
            ArenaDatabase.Database.Migrate();
            Commands.Init(ArenaDatabase,this);
            pluginName = "Arena";
        }
        public override Boolean hasCommands()
        {
            return true;
        }
    }
}
