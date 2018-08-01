using System.Collections.Generic;

namespace Server.Database {
    public class Clan {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }

        public virtual ICollection<User> Members { get; set; }
    }
}