using Microsoft.EntityFrameworkCore;

namespace Web_API.Data
{
    public class PokemonDBContext : DbContext
    {
        public DbSet<Pokemon> Pokemons { get; set; }
        public PokemonDBContext(DbContextOptions<PokemonDBContext> options) : base(options)
        {
        }
    }
}
