using System.Collections.Generic;

namespace Server.Database {
    public class User {
        public uint Id { get; set; }
        
        public string Name { get; set; }
        
        public byte[] PasswordHash { get; set; }

        public string Email { get; set; }
        public byte Permission { get; set; }
        
        public uint? ClanId { get; set; }
        public virtual Clan Clan { get; set; }
        
        public virtual Login Login { get; set; }

        public virtual ICollection<Ban> Bans { get; set; }

        public User() { }
        public User(string username, string email, string password) {
            this.Name = username;
            this.Email = email;
            this.PasswordHash = Hashing.Hash(password);
        }
        
        public bool VerifyPassword(string password) {
            return Hashing.Verify(password, this.PasswordHash);
        }
    }
}