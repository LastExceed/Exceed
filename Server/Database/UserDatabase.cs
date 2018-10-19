using Microsoft.EntityFrameworkCore;
using Resources;
using System;
using System.Linq;

namespace Server.Database {
    // To update Database run add-migration "update description"
    public class UserDatabase : DbContext {
        const string dbFileName = "db.sqlite";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite($"Data Source={dbFileName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            {
                var user = modelBuilder.Entity<User>();
                user.HasKey(x => x.Id);
                user.Property(x => x.Name).IsRequired();
                var password = user.Property(x => x.PasswordHash);
                password.IsRequired();
                password.HasMaxLength(Hashing.SaltSize + Hashing.HashSize);

                user.Property(x => x.Email).IsRequired();


                user.HasOne(x => x.Clan).WithMany(x => x.Members).HasForeignKey(x => x.ClanId);
                user.HasMany(x => x.Bans).WithOne(x => x.User).HasForeignKey(x => x.UserId);
                user.HasOne(x => x.Login).WithOne(x => x.User).HasForeignKey<Login>(x => x.UserId);
            }

            {
                var clans = modelBuilder.Entity<Clan>();
                clans.HasKey(x => x.Id);
                clans.Property(x => x.Name).IsRequired();
            }

            {
                var logins = modelBuilder.Entity<Login>();
                logins.HasKey(x => x.UserId);

            }

            {
                var bans = modelBuilder.Entity<Ban>();
                bans.HasKey(x => x.Id);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Ban> Bans { get; set; }

        public AuthResponse AuthUser(string username, string password, int ip, string mac) {
            var user = Users.Include(x => x.Login).SingleOrDefault(x => x.Name == username);
            if(user == null)
                return AuthResponse.UnknownUser;
            if(user.Login != null)
                return AuthResponse.UserAlreadyLoggedIn;

            if(Bans.Any(x => x.UserId == user.Id || x.Ip == ip || x.Mac == mac))
                return AuthResponse.Banned;

            if(user.VerifyPassword(password))
                return AuthResponse.Success;

            return AuthResponse.WrongPassword;
        }

        public RegisterResponse RegisterUser(string username, string email, string password) {
            if(Users.Any(x => x.Email == email)) {
                return RegisterResponse.EmailTaken;
            }
            if(Users.Any(x => x.Name == username)) {
                return RegisterResponse.UsernameTaken;
            }
            var user = new User(username, email, password);
            Users.Add(user);
            SaveChanges();

            return RegisterResponse.Success;
        }

        public void BanUser(string entityName, int ipAddress, string targetMac, string reason) {
            var user = Users.SingleOrDefault(x => x.Name == entityName)?.Id;
            var ban = new Ban { Ip = ipAddress, Mac = targetMac, Reason = reason, UserId = user };
            Bans.Add(ban);
            SaveChanges();
        }
        public PromoteResponse PromoteUser(string entityName, RoleID roleId)
        {
            var user = Users.SingleOrDefault(x => x.Name == entityName);
            if (user != null)
            {
                user.Permission = (byte)roleId;
                SaveChanges();
                return PromoteResponse.Success;
            }
            else
            {
                return PromoteResponse.InvalidTarget;
            }
        }
        public byte getRoleId(string entityName)
        {
            var user = Users.SingleOrDefault(x => x.Name == entityName);
            return user.Permission;
        }
    }
}