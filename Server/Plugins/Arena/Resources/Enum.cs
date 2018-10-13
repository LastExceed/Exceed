using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins.Arena.Resources
{
    public enum ArenaResponse : byte
    {
        Null,
        SetupLaunched,
        SetupAlreadyLaunched,
        SetupEmpty,
        SetupNameSet,
        SetupPositionSet,
        SetupIncomplete,
        ArenaCreated,
        ArenaDeleted,
        ArenaNotFound,
        ArenaListEmpty,
        ArenaReset
    }
    public enum ArenaPositionName : byte
    {
        player1,
        player2,
        spectator
    }
}
