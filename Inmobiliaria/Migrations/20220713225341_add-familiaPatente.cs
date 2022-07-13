using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class addfamiliaPatente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Familia_Patente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamiliaId = table.Column<int>(type: "int", nullable: false),
                    PatenteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familia_Patente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Familia_Patente_Familias_FamiliaId",
                        column: x => x.FamiliaId,
                        principalTable: "Familias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Familia_Patente_Patentes_PatenteId",
                        column: x => x.PatenteId,
                        principalTable: "Patentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Familia_Patente_FamiliaId",
                table: "Familia_Patente",
                column: "FamiliaId");

            migrationBuilder.CreateIndex(
                name: "IX_Familia_Patente_PatenteId",
                table: "Familia_Patente",
                column: "PatenteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Familia_Patente");
        }
    }
}
