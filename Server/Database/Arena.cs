using System.Collections.Generic;

namespace Server.Database {
    public class Arena {
        public uint Id { get; set; }
        public uint ArenaId { get; set; }
        public string Name { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }
    }
}