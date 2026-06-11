using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Pokedex.Infrastructure;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PokedexDbContext>
{
    public PokedexDbContext CreateDbContext(string[] args)
    {
        var currentDir = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(currentDir)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("pokedex")
            ?? configuration.GetConnectionString("PokedexDatabase")
            ?? "Host=localhost;Port=5432;Database=pokedex;Username=postgres;Password=postgres;SslMode=Disable";

        var optionsBuilder = new DbContextOptionsBuilder<PokedexDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(PokedexDbContext).Assembly.FullName);
            npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "public");
            npgsqlOptions.CommandTimeout(60);
            npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });

        return new PokedexDbContext(optionsBuilder.Options);
    }
}
