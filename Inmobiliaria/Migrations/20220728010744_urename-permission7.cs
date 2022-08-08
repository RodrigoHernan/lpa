using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class urenamepermission7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_AspNetUsers_ApplicationUserId",
                table: "UserPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_Permisos_PermisoId",
                table: "UserPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermission",
                table: "UserPermission");

            migrationBuilder.RenameTable(
                name: "UserPermission",
                newName: "UserPermissions");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermission_PermisoId",
                table: "UserPermissions",
                newName: "IX_UserPermissions_PermisoId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermission_ApplicationUserId",
                table: "UserPermissions",
                newName: "IX_UserPermissions_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_AspNetUsers_ApplicationUserId",
                table: "UserPermissions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_Permisos_PermisoId",
                table: "UserPermissions",
                column: "PermisoId",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_AspNetUsers_ApplicationUserId",
                table: "UserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_Permisos_PermisoId",
                table: "UserPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions");

            migrationBuilder.RenameTable(
                name: "UserPermissions",
                newName: "UserPermission");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermissions_PermisoId",
                table: "UserPermission",
                newName: "IX_UserPermission_PermisoId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermissions_ApplicationUserId",
                table: "UserPermission",
                newName: "IX_UserPermission_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermission",
                table: "UserPermission",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_AspNetUsers_ApplicationUserId",
                table: "UserPermission",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_Permisos_PermisoId",
                table: "UserPermission",
                column: "PermisoId",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
