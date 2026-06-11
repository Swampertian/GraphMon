using Microsoft.EntityFrameworkCore;
using Pokedex.Domain.Entities;
using Pokedex.Infrastructure.EntityConfigurations;

namespace Pokedex.Infrastructure;

public class PokedexDbContext : DbContext
{
    public PokedexDbContext(DbContextOptions<PokedexDbContext> options) : base(options) { }

    public DbSet<PokemonGeneric> Pokemons => Set<PokemonGeneric>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PokemonGenericConfigurations());
    }
}