using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class userpermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_FamiliasPatente_Familias_FamiliaId",
            //     table: "FamiliasPatente");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_FamiliasPatente_Patentes_PatenteId",
            //     table: "FamiliasPatente");

            migrationBuilder.CreateTable(
                name: "UserPermisos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermisoId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Table = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermisos", x => x.Id);
                });

            // migrationBuilder.AddForeignKey(
            //     name: "FK_FamiliasPatente_Familias_FamiliaId",
            //     table: "FamiliasPatente",
            //     column: "FamiliaId",
            //     principalTable: "Familias",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_FamiliasPatente_Patentes_PatenteId",
            //     table: "FamiliasPatente",
            //     column: "PatenteId",
            //     principalTable: "Patentes",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamiliasPatente_Familias_FamiliaId",
                table: "FamiliasPatente");

            migrationBuilder.DropForeignKey(
                name: "FK_FamiliasPatente_Patentes_PatenteId",
                table: "FamiliasPatente");

            migrationBuilder.DropTable(
                name: "UserPermisos");

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliasPatente_Familias_FamiliaId",
                table: "FamiliasPatente",
                column: "FamiliaId",
                principalTable: "Familias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliasPatente_Patentes_PatenteId",
                table: "FamiliasPatente",
                column: "PatenteId",
                principalTable: "Patentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
