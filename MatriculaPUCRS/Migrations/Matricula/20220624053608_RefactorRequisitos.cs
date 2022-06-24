using Microsoft.EntityFrameworkCore.Migrations;

namespace MatriculaPUCRS.Migrations.Matricula
{
    public partial class RefactorRequisitos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requisitos");

            migrationBuilder.CreateTable(
                name: "DisciplinaDisciplina",
                columns: table => new
                {
                    DisciplinaOrigemId = table.Column<long>(type: "bigint", nullable: false),
                    RequisitosId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisciplinaDisciplina", x => new { x.DisciplinaOrigemId, x.RequisitosId });
                    table.ForeignKey(
                        name: "FK_DisciplinaDisciplina_Disciplinas_DisciplinaOrigemId",
                        column: x => x.DisciplinaOrigemId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisciplinaDisciplina_Disciplinas_RequisitosId",
                        column: x => x.RequisitosId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinaDisciplina_RequisitosId",
                table: "DisciplinaDisciplina",
                column: "RequisitosId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisciplinaDisciplina");

            migrationBuilder.CreateTable(
                name: "Requisitos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisciplinaId = table.Column<long>(type: "bigint", nullable: false),
                    DisciplinaOrigemId = table.Column<long>(type: "bigint", nullable: false),
                    TipoRequisito = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitos", x => x.Id);
                    table.UniqueConstraint("AK_Requisitos_DisciplinaId_DisciplinaOrigemId", x => new { x.DisciplinaId, x.DisciplinaOrigemId });
                    table.ForeignKey(
                        name: "FK_Requisitos_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requisitos_Disciplinas_DisciplinaOrigemId",
                        column: x => x.DisciplinaOrigemId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requisitos_DisciplinaOrigemId",
                table: "Requisitos",
                column: "DisciplinaOrigemId");
        }
    }
}
