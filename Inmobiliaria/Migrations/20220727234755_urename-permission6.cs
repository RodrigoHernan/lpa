using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class urenamepermission6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_PermisoId",
                table: "UserPermission",
                column: "PermisoId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_Permisos_PermisoId",
                table: "UserPermission",
                column: "PermisoId",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_Permisos_PermisoId",
                table: "UserPermission");

            migrationBuilder.DropIndex(
                name: "IX_UserPermission_PermisoId",
                table: "UserPermission");
        }
    }
}
