using HotChocolate.Data;
using Pokedex.Application.DTOs;
using Pokedex.Domain.Interfaces;

using PokemonGenericEntity = Pokedex.Domain.Entities.PokemonGeneric;

namespace Pokedex.Application.Resolvers;


public sealed class QueryPokemonGenericResolver
{
   [GraphQLName("PokemonGeneric")]

   public Task <PokemonDataDto> GetPokemons([Service] IPokemonRepository repository,CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var pokemonsQuery = repository.Query();

        var pokemonEntities = pokemonsQuery
            .OrderByDescending(x => x.Number)
            .ToList();

        var pokemonResults = pokemonEntities.Select(MapToPokemonResultDto).ToList();

        var result = new PokemonDataDto
        {
            Data = pokemonResults
        };
        return Task.FromResult(result);
    }

    private static PokemonResultDto MapToPokemonResultDto(PokemonGenericEntity entity)
    {
        return new PokemonResultDto
        {
            Number = entity.Number,
            Name = entity.Name,
            Type1 = entity.Type1,
            Type2 = entity.Type2,
            Total = entity.Total,
            Hp = entity.Hp,
            Attack = entity.Attack,
            Defense = entity.Defense,
            SpAttack = entity.Sp_Attack,
            SpDefense = entity.Sp_Defense,
            Speed = entity.Speed,
            Generation = entity.Generation,
            Legendary = entity.Legendary
        };
    }
}
