using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class urenamepermission2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_AspNetUsers_ApplicationUserId",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_ApplicationUserId",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Permisos");

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PermisoId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermission_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_ApplicationUserId",
                table: "UserPermission",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Permisos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_ApplicationUserId",
                table: "Permisos",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_AspNetUsers_ApplicationUserId",
                table: "Permisos",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
