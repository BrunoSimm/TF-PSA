using Microsoft.EntityFrameworkCore.Migrations;

namespace MatriculaPUCRS.Migrations.Matricula
{
    public partial class UpdateSemestreTitulo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Semestres_Titulo",
                table: "Semestres");

            migrationBuilder.CreateIndex(
                name: "IX_Semestres_Titulo",
                table: "Semestres",
                column: "Titulo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Semestres_Titulo",
                table: "Semestres");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Semestres_Titulo",
                table: "Semestres",
                column: "Titulo");
        }
    }
}
