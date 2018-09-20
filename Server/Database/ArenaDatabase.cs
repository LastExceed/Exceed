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

        public Arena FindArena(uint id = 0)
        {
            var arena = Arenas.SingleOrDefault(x => x.ArenaId == id);
            return arena;
        }
        public ArenaResponse AddDuelArena(long[] position,string name = null)
        {
            if(name == null)
            {
                name = "arena" + Arenas.Count().ToString();
            }
            var arena = new Arena {ArenaId = (uint)Arenas.Count(),Name = name, X = position[0], Y = position[1], Z = position[2] };
            Arenas.Add(arena);
            SaveChanges();

            return ArenaResponse.ArenaCreated;
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
                Console.WriteLine(String.Format("Arena {0} : Name -> {1} , X -> {2}, Y -> {3}, Z -> {4}", arena.ArenaId, arena.Name, arena.X, arena.Y, arena.Z));
            }
        }
    }
}