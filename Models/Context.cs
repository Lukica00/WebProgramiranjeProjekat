using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class Context : DbContext
    {
        public DbSet<Bolnica> Bolnica { get; set; }
        public DbSet<Pacijent> Pacijent { get; set; }
        public DbSet<Lekar> Lekar { get; set; }
        public DbSet<Lecenje> Lecenje { get; set; }
        public Context(DbContextOptions options) : base(options)
        {

        }
    }
}