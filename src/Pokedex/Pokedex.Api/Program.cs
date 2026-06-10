using Pokedex.Application.DTOs;
using Pokedex.Application.Resolvers;
using Pokedex.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddGraphQLServer()
    .AddDocumentFromFile("../Pokedex.Infrastructure/Schema/pokedex.graphql")
    .AddResolver<QueryPokemonGenericResolver>("Query")
    .BindRuntimeType<PokemonDataDto>("PokemonData")
    .BindRuntimeType<PokemonResultDto>("PokemonResult");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PokedexDbContext>();
    db.Database.Migrate();
}

app.MapGraphQL("/pokedex");
app.MapDefaultEndpoints();

app.Run();