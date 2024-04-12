
namespace Project.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}