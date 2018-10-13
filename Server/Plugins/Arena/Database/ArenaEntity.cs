using Server.Plugins.Arena.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Plugins.Arena.Database
{
    public class ArenaEntity
    {
        public uint Id { get; set; }
        public uint ArenaId { get; set; }
        public string Name { get; set; }
        public byte[] SpawnPosition1 { get; set; }
        public byte[] SpawnPosition2 { get; set; }
        public byte[] SpawnPosition3 { get; set; }

        public ArenaEntity()
        {
        }
        public ArenaEntity(uint ArenaId, string Name)
        {
            this.ArenaId = ArenaId;
            this.Name = Name;
        }
        public long[] getPosition(ArenaPositionName arenaPositionName)
        {
            var position = ArenaDatabase.decodePosition(SpawnPosition1);
            switch (arenaPositionName)
            {
                case ArenaPositionName.player1:
                    position = ArenaDatabase.decodePosition(SpawnPosition1);
                    break;
                case ArenaPositionName.player2:
                    position = ArenaDatabase.decodePosition(SpawnPosition2);
                    break;
                case ArenaPositionName.spectator:
                    position = ArenaDatabase.decodePosition(SpawnPosition3);
                    break;
            }
            return position;
        }
    }
}
