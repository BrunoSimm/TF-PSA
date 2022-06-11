using Microsoft.EntityFrameworkCore.Migrations;

namespace MatriculaPUCRS.Migrations.Matricula
{
    public partial class HorarioGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HorarioFinal",
                table: "HorariosGrade",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HorarioInicial",
                table: "HorariosGrade",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorarioFinal",
                table: "HorariosGrade");

            migrationBuilder.DropColumn(
                name: "HorarioInicial",
                table: "HorariosGrade");
        }
    }
}
