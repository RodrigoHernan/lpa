using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class DHV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VerticalCheckDigits",
                table: "VerticalCheckDigits");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VerticalCheckDigits");

            migrationBuilder.AlterColumn<string>(
                name: "Entity",
                table: "VerticalCheckDigits",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerticalCheckDigits",
                table: "VerticalCheckDigits",
                column: "Entity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VerticalCheckDigits",
                table: "VerticalCheckDigits");

            migrationBuilder.AlterColumn<string>(
                name: "Entity",
                table: "VerticalCheckDigits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VerticalCheckDigits",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerticalCheckDigits",
                table: "VerticalCheckDigits",
                column: "Id");
        }
    }
}
