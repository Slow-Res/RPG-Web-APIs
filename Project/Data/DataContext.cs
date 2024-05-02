
using System.Reflection.Metadata;

namespace Project.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
        public DbSet<Skill> Skills => Set<Skill>();

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var list = new List<Skill>()
            {
                new() { Id = 1, Name = "Fireball", Damage = 30 },
                new() { Id = 2, Name = "Freez", Damage = 25 },
                new() { Id = 3, Name = "Thunder", Damage = 32 },
            };
            modelBuilder.Entity<Skill>().HasData(list);
        }
    }
}