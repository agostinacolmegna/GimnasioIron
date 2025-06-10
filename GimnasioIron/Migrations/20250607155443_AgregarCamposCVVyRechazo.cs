using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimnasioIron.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposCVVyRechazo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVV",
                table: "Pagos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroTarjeta",
                table: "Pagos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoRechazo",
                table: "Pagos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVV",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "NumeroTarjeta",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "MotivoRechazo",
                table: "Pagos");
        }
    }
}
