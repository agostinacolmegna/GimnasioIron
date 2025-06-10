using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimnasioIron.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposAClase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CupoMaximo",
                table: "Clases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Clases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Profesor",
                table: "Clases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CupoMaximo",
                table: "Clases");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Clases");

            migrationBuilder.DropColumn(
                name: "Profesor",
                table: "Clases");
        }
    }
}
