using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class addfamiliaPatente2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Familia_Patente_Familias_FamiliaId",
                table: "Familia_Patente");

            migrationBuilder.DropForeignKey(
                name: "FK_Familia_Patente_Patentes_PatenteId",
                table: "Familia_Patente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Familia_Patente",
                table: "Familia_Patente");

            migrationBuilder.RenameTable(
                name: "Familia_Patente",
                newName: "FamiliasPatente");

            migrationBuilder.RenameIndex(
                name: "IX_Familia_Patente_PatenteId",
                table: "FamiliasPatente",
                newName: "IX_FamiliasPatente_PatenteId");

            migrationBuilder.RenameIndex(
                name: "IX_Familia_Patente_FamiliaId",
                table: "FamiliasPatente",
                newName: "IX_FamiliasPatente_FamiliaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamiliasPatente",
                table: "FamiliasPatente",
                column: "Id");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_FamiliasPatente_Familias_FamiliaId",
            //     table: "FamiliasPatente",
            //     column: "FamiliaId",
            //     principalTable: "Familias",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_FamiliasPatente_Patentes_PatenteId",
            //     table: "FamiliasPatente",
            //     column: "PatenteId",
            //     principalTable: "Patentes",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamiliasPatente_Familias_FamiliaId",
                table: "FamiliasPatente");

            migrationBuilder.DropForeignKey(
                name: "FK_FamiliasPatente_Patentes_PatenteId",
                table: "FamiliasPatente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamiliasPatente",
                table: "FamiliasPatente");

            migrationBuilder.RenameTable(
                name: "FamiliasPatente",
                newName: "Familia_Patente");

            migrationBuilder.RenameIndex(
                name: "IX_FamiliasPatente_PatenteId",
                table: "Familia_Patente",
                newName: "IX_Familia_Patente_PatenteId");

            migrationBuilder.RenameIndex(
                name: "IX_FamiliasPatente_FamiliaId",
                table: "Familia_Patente",
                newName: "IX_Familia_Patente_FamiliaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Familia_Patente",
                table: "Familia_Patente",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Familia_Patente_Familias_FamiliaId",
                table: "Familia_Patente",
                column: "FamiliaId",
                principalTable: "Familias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Familia_Patente_Patentes_PatenteId",
                table: "Familia_Patente",
                column: "PatenteId",
                principalTable: "Patentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
