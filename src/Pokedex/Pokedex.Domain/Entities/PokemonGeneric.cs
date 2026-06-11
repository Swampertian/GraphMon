// 1.
namespace Pokedex.Domain.Entities;


public class PokemonGeneric
{
    public short Id { get; set; }
    public int Number { get; set; }
    public string? Name{ get; set; }
    public string? Type1 { get; set; }
    public string? Type2 { get; set; }
    public int? Total { get; set; }
    public int? Hp { get; set; }
    public int? Attack { get; set; }
    public int? Defense { get; set; }
    public int? Sp_Attack { get; set; }
    public int? Sp_Defense { get; set; }
    public int? Speed { get; set; }
    public int? Generation { get; set; }
    public bool? Legendary { get; set; }
}