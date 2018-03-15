using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Database {
    public class Clan {
        [Key] public uint id { get; set; }
        public string name { get; set; }
        public string tag { get; set; }

        public virtual ICollection<User> members { get; set; }
    }
}