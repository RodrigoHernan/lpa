using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class urenamepermission5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermisoId",
                table: "Permisos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_PermisoId",
                table: "Permisos",
                column: "PermisoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_Permisos_PermisoId",
                table: "Permisos",
                column: "PermisoId",
                principalTable: "Permisos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_Permisos_PermisoId",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_PermisoId",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "PermisoId",
                table: "Permisos");
        }
    }
}
