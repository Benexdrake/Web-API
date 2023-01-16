using Microsoft.EntityFrameworkCore;

namespace Web_API.Data
{
    public class CrunchyrollDBContext : DbContext
    {
        public DbSet<Anime> Animes { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        public CrunchyrollDBContext(DbContextOptions<CrunchyrollDBContext> options) : base(options)
        {
        }
    }
}
