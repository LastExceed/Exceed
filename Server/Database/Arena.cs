using Resources;
using System.Collections.Generic;

namespace Server.Database {
    public class Arena {
        public uint Id { get; set; }
        public uint ArenaId { get; set; }
        public string Name { get; set; }
        public byte[] SpawnPosition1 { get; set; }
        public byte[] SpawnPosition2 { get; set; }
        public byte[] SpawnPosition3 { get; set; }

        public Arena(uint ArenaId,string Name)
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