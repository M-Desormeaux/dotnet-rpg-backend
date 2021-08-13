using Microsoft.EntityFrameworkCore;
using NetRPG.Models;

namespace NetRPG.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }

        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill {ID = 1, Name = "Fireball", Damage = 25},
                new Skill {ID = 2, Name = "Acid Splash", Damage = 15},
                new Skill {ID = 3, Name = "Ice Storm", Damage = 50},
                new Skill {ID = 4, Name = "Meteor", Damage = 90},
                new Skill {ID = 5, Name = "Echo", Damage = 5},
                new Skill {ID = 6, Name = "Madness", Damage = 35},
                new Skill {ID = 7, Name = "Shock", Damage = 20}
            );
        }
    }
}