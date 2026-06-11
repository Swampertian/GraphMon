using Microsoft.EntityFrameworkCore;
using Pokedex.Domain.Entities;
using Pokedex.Domain.Interfaces;

namespace Pokedex.Infrastructure.Repositories;

public class PokemonRepository(PokedexDbContext context) : IPokemonRepository
{
    public IQueryable<PokemonGeneric> Query() => context.Pokemons.AsQueryable();

}