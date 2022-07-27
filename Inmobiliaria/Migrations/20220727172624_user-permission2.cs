using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    public partial class userpermission2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_FamiliasPatente_Familias_FamiliaId",
            //     table: "FamiliasPatente");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_FamiliasPatente_Patentes_PatenteId",
            //     table: "FamiliasPatente");

            migrationBuilder.DropTable(
                name: "Familias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patentes",
                table: "Patentes");

            migrationBuilder.RenameTable(
                name: "Patentes",
                newName: "PermisoModel");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "PermisoModel",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PermisoModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermisoModel",
                table: "PermisoModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PermisoModel_ApplicationUserId",
                table: "PermisoModel",
                column: "ApplicationUserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermisoModel",
                table: "PermisoModel");

            migrationBuilder.DropIndex(
                name: "IX_PermisoModel_ApplicationUserId",
                table: "PermisoModel");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "PermisoModel");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PermisoModel");

            migrationBuilder.RenameTable(
                name: "PermisoModel",
                newName: "Patentes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patentes",
                table: "Patentes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Familias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familias", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliasPatente_Familias_FamiliaId",
                table: "FamiliasPatente",
                column: "FamiliaId",
                principalTable: "Familias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliasPatente_Patentes_PatenteId",
                table: "FamiliasPatente",
                column: "PatenteId",
                principalTable: "Patentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
