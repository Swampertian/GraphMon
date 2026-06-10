using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokedex.Domain.Entities;

namespace Pokedex.Infrastructure.EntityConfigurations;

public sealed class PokemonGenericConfigurations : IEntityTypeConfiguration<PokemonGeneric>
{
    public void Configure(EntityTypeBuilder<PokemonGeneric> entity)
    {
        entity.ToTable("pokemon_generic");
        entity.HasKey(x => x.Number);

        entity.Property(x => x.Number).HasColumnName("number").ValueGeneratedNever();
        entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(100);
        entity.Property(x => x.Type1).HasColumnName("type1").HasMaxLength(50);
        entity.Property(x => x.Type2).HasColumnName("type2").HasMaxLength(50);
        entity.Property(x => x.Total).HasColumnName("total");
        entity.Property(x => x.Hp).HasColumnName("hp");
        entity.Property(x => x.Attack).HasColumnName("attack");
        entity.Property(x => x.Defense).HasColumnName("defense");
        entity.Property(x => x.Sp_Attack).HasColumnName("spattack");
        entity.Property(x => x.Sp_Defense).HasColumnName("spdefense");
        entity.Property(x => x.Speed).HasColumnName("speed");
        entity.Property(x => x.Generation).HasColumnName("generation");
        entity.Property(x => x.Legendary).HasColumnName("legendary");
    }
}