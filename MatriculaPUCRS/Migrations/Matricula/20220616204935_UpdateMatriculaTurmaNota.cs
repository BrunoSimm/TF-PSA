using Microsoft.EntityFrameworkCore.Migrations;

namespace MatriculaPUCRS.Migrations.Matricula
{
    public partial class UpdateMatriculaTurmaNota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aprovado",
                table: "MatriculaTurmas");

            migrationBuilder.AlterColumn<float>(
                name: "Nota",
                table: "MatriculaTurmas",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Nota",
                table: "MatriculaTurmas",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Aprovado",
                table: "MatriculaTurmas",
                type: "bit",
                nullable: false,
                computedColumnSql: "CASE WHEN MatriculaTurmas.Nota >= 5 THEN CAST(1 as BIT) ELSE CAST(0 as BIT) END");
        }
    }
}
