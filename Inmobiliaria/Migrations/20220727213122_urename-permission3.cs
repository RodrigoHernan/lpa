using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class urenamepermission3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_AspNetUsers_ApplicationUserId",
                table: "UserPermission");

            migrationBuilder.DropIndex(
                name: "IX_UserPermission_ApplicationUserId",
                table: "UserPermission");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserPermission");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "UserPermission",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "UserPermission",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_ApplicationUserId1",
                table: "UserPermission",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_AspNetUsers_ApplicationUserId1",
                table: "UserPermission",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_AspNetUsers_ApplicationUserId1",
                table: "UserPermission");

            migrationBuilder.DropIndex(
                name: "IX_UserPermission_ApplicationUserId1",
                table: "UserPermission");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "UserPermission");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "UserPermission",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserPermission",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_ApplicationUserId",
                table: "UserPermission",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_AspNetUsers_ApplicationUserId",
                table: "UserPermission",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
