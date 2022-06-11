using Microsoft.EntityFrameworkCore.Migrations;

namespace MatriculaPUCRS.Migrations.Matricula
{
    public partial class Requisitos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisitos_Disciplinas_DisciplinaId",
                table: "Requisitos");

            migrationBuilder.DropIndex(
                name: "IX_Requisitos_DisciplinaId",
                table: "Requisitos");

            migrationBuilder.RenameColumn(
                name: "EstadoEstudanteEnum",
                table: "Estudantes",
                newName: "Estado");

            migrationBuilder.AddColumn<long>(
                name: "DisciplinaOrigemId",
                table: "Requisitos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "MatriculaTurmas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Nivel",
                table: "Disciplinas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Requisitos_DisciplinaId_DisciplinaOrigemId",
                table: "Requisitos",
                columns: new[] { "DisciplinaId", "DisciplinaOrigemId" });

            migrationBuilder.CreateIndex(
                name: "IX_Requisitos_DisciplinaOrigemId",
                table: "Requisitos",
                column: "DisciplinaOrigemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitos_Disciplinas_DisciplinaId",
                table: "Requisitos",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitos_Disciplinas_DisciplinaOrigemId",
                table: "Requisitos",
                column: "DisciplinaOrigemId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisitos_Disciplinas_DisciplinaId",
                table: "Requisitos");

            migrationBuilder.DropForeignKey(
                name: "FK_Requisitos_Disciplinas_DisciplinaOrigemId",
                table: "Requisitos");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Requisitos_DisciplinaId_DisciplinaOrigemId",
                table: "Requisitos");

            migrationBuilder.DropIndex(
                name: "IX_Requisitos_DisciplinaOrigemId",
                table: "Requisitos");

            migrationBuilder.DropColumn(
                name: "DisciplinaOrigemId",
                table: "Requisitos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "MatriculaTurmas");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Disciplinas");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Estudantes",
                newName: "EstadoEstudanteEnum");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitos_DisciplinaId",
                table: "Requisitos",
                column: "DisciplinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisitos_Disciplinas_DisciplinaId",
                table: "Requisitos",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
