using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pokedex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSurrogateIdToPokemonGeneric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_pokemon_generic",
                table: "pokemon_generic");

            migrationBuilder.AddColumn<short>(
                name: "id",
                table: "pokemon_generic",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_pokemon_generic",
                table: "pokemon_generic",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_pokemon_generic",
                table: "pokemon_generic");

            migrationBuilder.DropColumn(
                name: "id",
                table: "pokemon_generic");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pokemon_generic",
                table: "pokemon_generic",
                column: "number");
        }
    }
}
