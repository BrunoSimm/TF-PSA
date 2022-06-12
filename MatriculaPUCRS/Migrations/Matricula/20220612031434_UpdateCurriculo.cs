using Microsoft.EntityFrameworkCore.Migrations;

namespace MatriculaPUCRS.Migrations.Matricula
{
    public partial class UpdateCurriculo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurriculoId",
                table: "Estudantes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CargaHoraria",
                table: "Disciplinas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NomeDoCurso",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Estudantes_CurriculoId",
                table: "Estudantes",
                column: "CurriculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudantes_Curriculos_CurriculoId",
                table: "Estudantes",
                column: "CurriculoId",
                principalTable: "Curriculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudantes_Curriculos_CurriculoId",
                table: "Estudantes");

            migrationBuilder.DropIndex(
                name: "IX_Estudantes_CurriculoId",
                table: "Estudantes");

            migrationBuilder.DropColumn(
                name: "CurriculoId",
                table: "Estudantes");

            migrationBuilder.DropColumn(
                name: "CargaHoraria",
                table: "Disciplinas");

            migrationBuilder.DropColumn(
                name: "NomeDoCurso",
                table: "Curriculos");
        }
    }
}
