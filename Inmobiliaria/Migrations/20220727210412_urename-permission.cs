using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class urenamepermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamiliasPatente_PermisoModel_FamiliaId",
                table: "FamiliasPatente");

            migrationBuilder.DropForeignKey(
                name: "FK_FamiliasPatente_PermisoModel_PatenteId",
                table: "FamiliasPatente");

            migrationBuilder.DropForeignKey(
                name: "FK_PermisoModel_AspNetUsers_ApplicationUserId",
                table: "PermisoModel");

            migrationBuilder.DropTable(
                name: "UserPermisos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermisoModel",
                table: "PermisoModel");

            migrationBuilder.RenameTable(
                name: "PermisoModel",
                newName: "Permisos");

            migrationBuilder.RenameIndex(
                name: "IX_PermisoModel_ApplicationUserId",
                table: "Permisos",
                newName: "IX_Permisos_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permisos",
                table: "Permisos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliasPatente_Permisos_FamiliaId",
                table: "FamiliasPatente",
                column: "FamiliaId",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliasPatente_Permisos_PatenteId",
                table: "FamiliasPatente",
                column: "PatenteId",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_AspNetUsers_ApplicationUserId",
                table: "Permisos",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamiliasPatente_Permisos_FamiliaId",
                table: "FamiliasPatente");

            migrationBuilder.DropForeignKey(
                name: "FK_FamiliasPatente_Permisos_PatenteId",
                table: "FamiliasPatente");

            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_AspNetUsers_ApplicationUserId",
                table: "Permisos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permisos",
                table: "Permisos");

            migrationBuilder.RenameTable(
                name: "Permisos",
                newName: "PermisoModel");

            migrationBuilder.RenameIndex(
                name: "IX_Permisos_ApplicationUserId",
                table: "PermisoModel",
                newName: "IX_PermisoModel_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermisoModel",
                table: "PermisoModel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserPermisos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermisoId = table.Column<int>(type: "int", nullable: false),
                    Table = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermisos", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliasPatente_PermisoModel_FamiliaId",
                table: "FamiliasPatente",
                column: "FamiliaId",
                principalTable: "PermisoModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliasPatente_PermisoModel_PatenteId",
                table: "FamiliasPatente",
                column: "PatenteId",
                principalTable: "PermisoModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermisoModel_AspNetUsers_ApplicationUserId",
                table: "PermisoModel",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
