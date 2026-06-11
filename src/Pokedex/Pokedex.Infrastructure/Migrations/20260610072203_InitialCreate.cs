using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokedex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pokemon_generic",
                columns: table => new
                {
                    number = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    type1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    type2 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    total = table.Column<int>(type: "integer", nullable: true),
                    hp = table.Column<int>(type: "integer", nullable: true),
                    attack = table.Column<int>(type: "integer", nullable: true),
                    defense = table.Column<int>(type: "integer", nullable: true),
                    spattack = table.Column<int>(type: "integer", nullable: true),
                    spdefense = table.Column<int>(type: "integer", nullable: true),
                    speed = table.Column<int>(type: "integer", nullable: true),
                    generation = table.Column<int>(type: "integer", nullable: true),
                    legendary = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pokemon_generic", x => x.number);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pokemon_generic");
        }
    }
}
