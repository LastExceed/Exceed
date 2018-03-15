using System.ComponentModel.DataAnnotations;

namespace Server.Database {
    public class Login {
        public int ip { get; set; }
        public string mac { get; set; }

        [Key] public uint userId { get; set; }
        public virtual User user { get; set; }
    }
}