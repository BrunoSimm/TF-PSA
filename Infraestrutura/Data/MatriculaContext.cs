using Entidades.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Data
{
    public class MatriculaContext : DbContext
    {
        public MatriculaContext(DbContextOptions<MatriculaContext> options) : base(options)
        {
        }
        public DbSet<Estudante> Estudantes { get; set; }
        public DbSet<Curriculo> Curriculos { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<MatriculaTurma> MatriculaTurmas { get; set; }
        public DbSet<Semestre> Semestres { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<HorarioGrade> HorariosGrade { get; set; }
        public DbSet<Requisito> Requisitos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curriculo>().HasAlternateKey("Codigo");
            modelBuilder.Entity<HorarioGrade>().HasAlternateKey("Horario");
            modelBuilder.Entity<Semestre>().HasAlternateKey("Titulo");
            modelBuilder.Entity<Turma>().Ignore(t => t.VagasRemanescentes);

            modelBuilder.Entity<MatriculaTurma>()
                .HasKey(m => new { m.TurmaId, m.EstudanteId });
        }
    }
}
