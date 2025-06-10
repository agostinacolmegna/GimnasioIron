using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimnasioIron.Migrations
{
    /// <inheritdoc />
    public partial class RenombrarCupoACupoMaximo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cupo",
                table: "Clases");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cupo",
                table: "Clases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
