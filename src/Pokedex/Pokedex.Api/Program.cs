
using Pokedex.Infrastructure;
using Pokedex.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

using Pokedex.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHotChocolateDefaults();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PokedexDbContext>();
    db.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<IPokemonSeeder>();
    await seeder.SeedAsync();
}

app.MapGraphQL("/pokedex");
app.MapDefaultEndpoints();

app.Run();