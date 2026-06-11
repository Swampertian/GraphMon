using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Pokedex.Domain.Interfaces;
using Pokedex.Infrastructure.Repositories;
using Pokedex.Infrastructure.Seed;

namespace Pokedex.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("pokedex") ?? throw new InvalidOperationException("Connection string 'pokedex' not found.");

        services.AddDbContext<PokedexDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IPokemonRepository, PokemonRepository>();
        services.AddScoped<IPokemonSeeder, PokemonGenericSeeder>();

        return services;
    }
}
