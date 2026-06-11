namespace Pokedex.Application.DTOs;

[GraphQLName("PokemonData")]
public sealed class PokemonDataDto
{
    public IReadOnlyList<PokemonResultDto> Data { get; init; } = [];
}
