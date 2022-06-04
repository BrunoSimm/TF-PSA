using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MatriculaPUCRS.Migrations.Matricula
{
    public partial class MatriculaDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Curriculos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculos", x => x.Id);
                    table.UniqueConstraint("AK_Curriculos_Codigo", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Disciplinas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplinas", x => x.Id);
                    table.UniqueConstraint("AK_Disciplinas_Codigo", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Estudantes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DigitoVerificador = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoEstudanteEnum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosGrade",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Horario = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosGrade", x => x.Id);
                    table.UniqueConstraint("AK_HorariosGrade_Horario", x => x.Horario);
                });

            migrationBuilder.CreateTable(
                name: "Semestres",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataInicial = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFinal = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semestres", x => x.Id);
                    table.UniqueConstraint("AK_Semestres_Titulo", x => x.Titulo);
                });

            migrationBuilder.CreateTable(
                name: "CurriculoDisciplina",
                columns: table => new
                {
                    CurriculosId = table.Column<long>(type: "bigint", nullable: false),
                    DisciplinasId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculoDisciplina", x => new { x.CurriculosId, x.DisciplinasId });
                    table.ForeignKey(
                        name: "FK_CurriculoDisciplina_Curriculos_CurriculosId",
                        column: x => x.CurriculosId,
                        principalTable: "Curriculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculoDisciplina_Disciplinas_DisciplinasId",
                        column: x => x.DisciplinasId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requisitos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoRequisito = table.Column<int>(type: "int", nullable: false),
                    DisciplinaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requisitos_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turmas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroDeVagas = table.Column<int>(type: "int", nullable: false),
                    DisciplinaId = table.Column<long>(type: "bigint", nullable: false),
                    SemestreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turmas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turmas_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turmas_Semestres_SemestreId",
                        column: x => x.SemestreId,
                        principalTable: "Semestres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HorarioGradeTurma",
                columns: table => new
                {
                    HorariosId = table.Column<long>(type: "bigint", nullable: false),
                    TurmasId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioGradeTurma", x => new { x.HorariosId, x.TurmasId });
                    table.ForeignKey(
                        name: "FK_HorarioGradeTurma_HorariosGrade_HorariosId",
                        column: x => x.HorariosId,
                        principalTable: "HorariosGrade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HorarioGradeTurma_Turmas_TurmasId",
                        column: x => x.TurmasId,
                        principalTable: "Turmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatriculaTurmas",
                columns: table => new
                {
                    TurmaId = table.Column<long>(type: "bigint", nullable: false),
                    EstudanteId = table.Column<long>(type: "bigint", nullable: false),
                    Nota = table.Column<float>(type: "real", nullable: false),
                    Aprovado = table.Column<bool>(type: "bit", nullable: false, computedColumnSql: "CASE WHEN MatriculaTurmas.Nota >= 5 THEN CAST(1 as BIT) ELSE CAST(0 as BIT) END")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatriculaTurmas", x => new { x.TurmaId, x.EstudanteId });
                    table.ForeignKey(
                        name: "FK_MatriculaTurmas_Estudantes_EstudanteId",
                        column: x => x.EstudanteId,
                        principalTable: "Estudantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatriculaTurmas_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurriculoDisciplina_DisciplinasId",
                table: "CurriculoDisciplina",
                column: "DisciplinasId");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioGradeTurma_TurmasId",
                table: "HorarioGradeTurma",
                column: "TurmasId");

            migrationBuilder.CreateIndex(
                name: "IX_MatriculaTurmas_EstudanteId",
                table: "MatriculaTurmas",
                column: "EstudanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitos_DisciplinaId",
                table: "Requisitos",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_DisciplinaId",
                table: "Turmas",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_SemestreId",
                table: "Turmas",
                column: "SemestreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurriculoDisciplina");

            migrationBuilder.DropTable(
                name: "HorarioGradeTurma");

            migrationBuilder.DropTable(
                name: "MatriculaTurmas");

            migrationBuilder.DropTable(
                name: "Requisitos");

            migrationBuilder.DropTable(
                name: "Curriculos");

            migrationBuilder.DropTable(
                name: "HorariosGrade");

            migrationBuilder.DropTable(
                name: "Estudantes");

            migrationBuilder.DropTable(
                name: "Turmas");

            migrationBuilder.DropTable(
                name: "Disciplinas");

            migrationBuilder.DropTable(
                name: "Semestres");
        }
    }
}
