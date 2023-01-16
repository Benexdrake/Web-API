using Microsoft.EntityFrameworkCore;

namespace Web_API.Data
{
    public class IMDbDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public IMDbDBContext(DbContextOptions<IMDbDBContext> options) :base(options)
        {
        }
    }
}
