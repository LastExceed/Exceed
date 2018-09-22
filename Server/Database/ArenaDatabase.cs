using Microsoft.EntityFrameworkCore;
using Resources;
using System.Linq;
using System.IO;
using System;

namespace Server.Database {
    // To update Database run add-migration "update description"
    public class ArenaDatabase : DbContext {
        const string dbFileName = "db.sqlite";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite($"Data Source={dbFileName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {        
            {
                var arenas = modelBuilder.Entity<Arena>();
                arenas.HasKey(x => x.Id);
                arenas.Property(x => x.Name).IsRequired();
            }          
        }
        public DbSet<Arena> Arenas { get; set; }
        public Arena setupArena;
        public Arena FindArena(uint id = 0)
        {
            var arena = Arenas.SingleOrDefault(x => x.ArenaId == id);
            return arena;
        }
        public Arena FetchRandomArena()
        {
            int range = Arenas.Count();
            Random rnd = new Random();
            int id = rnd.Next(0, range);
            var arena = Arenas.SingleOrDefault(x => x.ArenaId == id);
            return arena;
        }
        public ArenaResponse ResetArena()
        {
            Arenas.RemoveRange(Arenas.ToList());
            SaveChanges();
            return ArenaResponse.ArenaReset;
        }
        public ArenaResponse launchSetupArena(string name = null)
        {
            if (setupArena == null)
            {
                if (name == null)
                {
                    name = "arena" + Arenas.Count().ToString();
                }
                setupArena = new Arena((uint)Arenas.Count(), name);
                return ArenaResponse.SetupLaunched;
            }
            return ArenaResponse.SetupAlreadyLaunched;
        }
        public ArenaResponse setNameArena(string arenaName = null)
        {
            if(setupArena != null)
            {
                if (arenaName == null)
                {
                    arenaName = "arena" + Arenas.Count().ToString();
                }
                setupArena.Name = arenaName;
                return ArenaResponse.SetupNameSet;
            }
            return ArenaResponse.SetupEmpty;
        }
        public ArenaResponse setPositionSetupArena(long[] position,ArenaPositionName arenaPositionName)
        {
            if(setupArena != null)
            {
                switch(arenaPositionName)
                {
                    case ArenaPositionName.player1:
                        setupArena.SpawnPosition1 = encodePosition(position);
                        break;
                    case ArenaPositionName.player2:
                        setupArena.SpawnPosition2 = encodePosition(position);
                        break;
                    case ArenaPositionName.spectator:
                        setupArena.SpawnPosition3 = encodePosition(position);
                        break;
                }
                return ArenaResponse.SetupPositionSet;
            }
            return ArenaResponse.SetupEmpty;
        }
        public ArenaResponse SaveDuelArena()
        {
            if(setupArena == null)
            {
                return ArenaResponse.SetupEmpty;
            }
            if (isValid(setupArena))
            {
                Arenas.Add(setupArena);
                SaveChanges();
                setupArena = null;
                return ArenaResponse.ArenaCreated;
            }
         return ArenaResponse.SetupIncomplete;
        }

        public ArenaResponse RemoveDuelArena(uint id = 0)
        {
            var arena = Arenas.SingleOrDefault(x => x.ArenaId == id);
            if (arena == null)
            {
                return ArenaResponse.ArenaNotFound;
            }
            Arenas.Remove(arena);
            SaveChanges();

            return ArenaResponse.ArenaDeleted;
        }
        public ArenaResponse RemoveDuelArena(string name = null)
        {
            var arena = Arenas.SingleOrDefault(x => x.Name == name);
            if(arena == null)
            {
                return ArenaResponse.ArenaNotFound;
            }
            Arenas.Remove(arena);
            SaveChanges();
            return ArenaResponse.ArenaDeleted;
        }
        public void listArena()
        {
            var arenas = Arenas.ToList();
            foreach( Arena arena in  arenas)
            {
                //Console.WriteLine(String.Format("Arena {0} : Name -> {1} , X -> {2}, Y -> {3}, Z -> {4}", arena.ArenaId, arena.Name, arena.X, arena.Y, arena.Z));
            }
        }
        public Boolean isValid(Arena arena)
        {
            if (arena.Name != null && arena.SpawnPosition1 != null && arena.SpawnPosition2 != null && arena.SpawnPosition3 != null)
            {
                if(arena.ArenaId == 0 && Arenas.Count() == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static byte[] encodePosition(long[] position)
        {
            byte[] encodedPosition = new byte[8*position.Length];
            Array.Copy(BitConverter.GetBytes(position[0]), 0, encodedPosition, 0,8);
            Array.Copy(BitConverter.GetBytes(position[1]), 0, encodedPosition, 8, 8);
            Array.Copy(BitConverter.GetBytes(position[2]), 0, encodedPosition, 16, 8);
            return encodedPosition;
        }
        public static long[] decodePosition(byte[] encodedPosition)
        {
            long[] decodedPosition = new long[3];
            decodedPosition[0] = BitConverter.ToInt64(encodedPosition, 0);
            decodedPosition[1] = BitConverter.ToInt64(encodedPosition, 8);
            decodedPosition[2] = BitConverter.ToInt64(encodedPosition, 16);
            return decodedPosition;
        }
    }
}