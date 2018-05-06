using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Database {
    public class User {
        [Key] public uint id { get; set; }

        [Required]
        public string name { get; set; }

        [Required, MaxLength(Hashing.SaltSize + Hashing.HashSize)]
        public byte[] PasswordHash { get; set; }

        [Required, DataType(DataType.EmailAddress)]
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
            this.PasswordHash = Hashing.Hash(password);
        }
        
        public bool VerifyPassword(string password) {
            return Hashing.Verify(password, this.PasswordHash);
        }
    }
}