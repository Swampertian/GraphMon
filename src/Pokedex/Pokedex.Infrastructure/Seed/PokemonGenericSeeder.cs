using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using Polly;
using Polly.Retry;

using Pokedex.Domain.Entities;

namespace Pokedex.Infrastructure.Seed;
public interface IPokemonSeeder
{
    Task SeedAsync();
}
public class PokemonGenericSeeder(
    PokedexDbContext db,
    IConfiguration configuration,
    ILogger<PokemonGenericSeeder> logger) : IPokemonSeeder
{
    public async Task SeedAsync()
    {
        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    logger.LogWarning("Seed retry {Attempt} after error: {Error}",
                    
                    args.AttemptNumber, args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        await pipeline.ExecuteAsync(async ct => await DoSeedAsync(ct));
    }

    private async Task DoSeedAsync(CancellationToken ct)
    {
        db.ChangeTracker.Clear();

        if (db.Pokemons.Any())
        {
            logger.LogInformation("Pokemons table already seeded. Skipping.");
            return;
        }

        var csvPath = configuration["SeedOptions:PokemonCsvPath"]
            ?? throw new InvalidOperationException("SeedOptions:PokemonCsvPath not configured.");

        var fullPath = Path.IsPathRooted(csvPath)
            ? csvPath
            : Path.Combine(Directory.GetCurrentDirectory(), csvPath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Pokemon CSV not found at: {fullPath}");

        var pokemons = ParseCsv(fullPath);
        
        await db.Pokemons.AddRangeAsync(pokemons, ct);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Seeded {Count} Pokémon records.", pokemons.Count);
    }

    private static List<PokemonGeneric> ParseCsv(string path)
    {
        var result = new List<PokemonGeneric>();

        using var parser = new TextFieldParser(path);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

       
        if (!parser.EndOfData) parser.ReadFields();

        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            if (fields is null || fields.Length < 13) continue;

            result.Add(new PokemonGeneric
            {
                Number     = int.Parse(fields[0]),
                Name       = fields[1],
                Type1      = fields[2],
                Type2      = string.IsNullOrWhiteSpace(fields[3]) ? null : fields[3],
                Total      = int.TryParse(fields[4], out var total)    ? total    : null,
                Hp         = int.TryParse(fields[5], out var hp)       ? hp       : null,
                Attack     = int.TryParse(fields[6], out var atk)      ? atk      : null,
                Defense    = int.TryParse(fields[7], out var def)      ? def      : null,
                Sp_Attack  = int.TryParse(fields[8], out var spa)      ? spa      : null,
                Sp_Defense = int.TryParse(fields[9], out var spd)      ? spd      : null,
                Speed      = int.TryParse(fields[10], out var spd2)    ? spd2     : null,
                Generation = int.TryParse(fields[11], out var gen)     ? gen      : null,
                Legendary  = fields[12].Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase)
            });
        }

        return result;
    }
}
