using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Database {
    public class User {
        [Key] public uint id { get; set; }

        public string name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public byte permission { get; set; }

        public uint? clanId { get; set; }
        public virtual Clan clan { get; set; }

        public virtual Login login { get; set; }

        public virtual ICollection<Ban> bans { get; set; }

        public User() { }
        public User(string username, string email, string password) {
            this.name = username;
            this.email = email;
            this.password = Hashing.Hash(password);
        }
        
        public bool VerifyPassword(string password) {
            return Hashing.Verify(password, this.password);
        }
    }
}