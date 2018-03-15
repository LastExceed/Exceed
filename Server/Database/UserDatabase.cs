using Microsoft.EntityFrameworkCore;
using Resources;
using System.Linq;

namespace Server.Database {
    public class UserDatabase : DbContext {
        const string dbFileName = "db.sqlite";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite($"Data Source={dbFileName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            var user = modelBuilder.Entity<User>();
            user.HasOne(x => x.clan).WithMany(x => x.members).HasForeignKey(x => x.clanId);
            user.HasMany(x => x.bans).WithOne(x => x.user).HasForeignKey(x => x.userId);
            user.HasOne(x => x.login).WithOne(x => x.user).HasForeignKey<Login>(x => x.userId);
        }

        public DbSet<User> users { get; set; }
        public DbSet<Clan> clans { get; set; }
        public DbSet<Login> logins { get; set; }
        public DbSet<Ban> bans { get; set; }

        public AuthResponse AuthUser(string username, string password, int ip, string mac) {
            var user = users.Include(x => x.login).SingleOrDefault(x => x.name == username);
            if(user == null)
                return AuthResponse.UnknownUser;
            if(user.login != null)
                return AuthResponse.UserAlreadyLoggedIn;

            if(bans.Any(x => x.userId == user.id || x.ip == ip || x.mac == mac))
                return AuthResponse.Banned;

            if(user.VerifyPassword(password))
                return AuthResponse.Success;

            return AuthResponse.WrongPassword;
        }

        public RegisterResponse RegisterUser(string username, string email, string password) {
            if(users.Any(x => x.email == email)) {
                return RegisterResponse.EmailTaken;
            }
            if(users.Any(x => x.name == username)) {
                return RegisterResponse.UsernameTaken;
            }
            var user = new User(username, email, password);
            users.Add(user);
            SaveChanges();

            return RegisterResponse.Success;
        }

        public void BanUser(string entityName, int ipAddress, string targetMac, string reason) {
            var user = users.SingleOrDefault(x => x.name == entityName)?.id;
            var ban = new Ban { ip = ipAddress, mac = targetMac, reason = reason, userId = user };
            bans.Add(ban);
            SaveChanges();
        }
    }
}