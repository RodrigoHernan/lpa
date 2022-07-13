using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class patente2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Patente",
                table: "Patente");

            migrationBuilder.RenameTable(
                name: "Patente",
                newName: "Patentes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patentes",
                table: "Patentes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Patentes",
                table: "Patentes");

            migrationBuilder.RenameTable(
                name: "Patentes",
                newName: "Patente");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patente",
                table: "Patente",
                column: "Id");
        }
    }
}
