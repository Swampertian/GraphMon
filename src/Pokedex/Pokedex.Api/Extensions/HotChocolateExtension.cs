using Pokedex.Application.DTOs;
using Pokedex.Application.Resolvers;


namespace Pokedex.Api.Extensions;

public static class HotChocolateExtensions
{
    public static IServiceCollection AddHotChocolateDefaults(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddDocumentFromFile("../Pokedex.Infrastructure/Schema/pokedex.graphql")
            .AddResolver<QueryPokemonGenericResolver>("Query")
            .BindRuntimeType<PokemonDataDto>("PokemonData")
            .BindRuntimeType<PokemonResultDto>("PokemonResult");

        return services;
    }
}