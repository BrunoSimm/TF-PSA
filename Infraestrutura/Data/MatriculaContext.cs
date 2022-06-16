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
            modelBuilder.Entity<Curriculo>().HasAlternateKey(c => c.Codigo);
            modelBuilder.Entity<Curriculo>().Property(c => c.NomeDoCurso).IsRequired();

            modelBuilder.Entity<Disciplina>().HasAlternateKey(d => d.Codigo);

            modelBuilder.Entity<HorarioGrade>().HasAlternateKey(hg => hg.Horario);

            modelBuilder.Entity<Semestre>().HasIndex(s => s.Titulo).IsUnique();
            modelBuilder.Entity<Semestre>().Property(s => s.Titulo).IsRequired();
            modelBuilder.Entity<Semestre>().Property(s => s.DataInicial).IsRequired();
            modelBuilder.Entity<Semestre>().Property(s => s.DataFinal).IsRequired();
            
            modelBuilder.Entity<Turma>().Ignore(t => t.VagasRemanescentes);

            modelBuilder.Entity<Estudante>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Estudante>().Property(e => e.Nome).IsRequired();
            modelBuilder.Entity<Estudante>().Property(e => e.CPF).IsRequired();
            modelBuilder.Entity<Estudante>().HasAlternateKey("CPF");
            modelBuilder.Entity<Estudante>().Property(e => e.Estado).HasConversion<string>();
            modelBuilder.Entity<Estudante>().Property(e => e.DigitoVerificador)
                .HasComputedColumnSql("CONVERT(INT, (Estudantes.Id % 9) + 1)"); // 1-9

            modelBuilder.Entity<MatriculaTurma>().HasKey(mt => new { mt.TurmaId, mt.EstudanteId });
            modelBuilder.Entity<MatriculaTurma>().Property(mt => mt.Estado).HasConversion<string>();

            modelBuilder.Entity<Disciplina>()
                .HasMany<Requisito>()
                .WithOne(r => r.Disciplina)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Requisito>().HasAlternateKey(r => new { r.DisciplinaId, r.DisciplinaOrigemId });
            modelBuilder.Entity<Requisito>().HasOne(r => r.Disciplina);
            modelBuilder.Entity<Requisito>().HasOne(r => r.DisciplinaOrigem);
        }
    }
}
