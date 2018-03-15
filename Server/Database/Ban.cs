using System.ComponentModel.DataAnnotations;

namespace Server.Database {
    public class Ban {
        [Key] public uint id { get; set; }
        public int ip { get; set; }
        public string mac { get; set; }
        public string reason { get; set; }

        public uint? userId { get; set; }
        public virtual User user { get; set; }
    }
}