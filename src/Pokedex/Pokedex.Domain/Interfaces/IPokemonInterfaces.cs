using Pokedex.Domain.Entities;

namespace Pokedex.Domain.Interfaces;

public interface IPokemonRepository
{
    IQueryable<PokemonGeneric> Query();
}
