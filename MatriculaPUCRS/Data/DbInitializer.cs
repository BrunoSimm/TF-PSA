using Entidades.Modelos;
using Infraestrutura.Data;
using MatriculaPUCRS.Areas.Roles;
using MatriculaPUCRS.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(MatriculaContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext contextIdentity)
        {
            contextIdentity.Database.EnsureCreated();
            context.Database.EnsureCreated();
            // Look for any students.
            if (context.Curriculos.Any())
            {
                return;   // DB has been seeded
            }

            if (!contextIdentity.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Coordenador.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Estudante.ToString()));
            }

            var curriculos = new Curriculo[]
            {
                new Curriculo { Codigo = "4/624", Ativo = true }
            };
            context.Curriculos.AddRange(curriculos);
            await context.SaveChangesAsync();

            var disciplinas = new Disciplina[]
            {
                new Disciplina { Codigo = "4611C-06", Nome = "Fundamentos de Programação", Curriculos = new Curriculo[] { curriculos[0] } },
                new Disciplina { Codigo = "95300-04", Nome = "Cálculo 1", Curriculos = new Curriculo[] { curriculos[0] } },
                new Disciplina { Codigo = "4647P-02", Nome = "Fundamentos da Computação", Curriculos = new Curriculo[] { curriculos[0] } },
                new Disciplina { Codigo = "4636U-04", Nome = "Introdução a Sistemas de Informação", Curriculos = new Curriculo[] { curriculos[0] } } ,
                new Disciplina { Codigo = "254PF-04", Nome = "Fundamentos Aplicados a Administração", Curriculos = new Curriculo[] { curriculos[0] } },
            };
            context.Disciplinas.AddRange(disciplinas);
            await context.SaveChangesAsync();

            var semestres = new Semestre[]
            {
                new Semestre { Titulo = "2020/1", DataInicial = new DateTime(2020,1,1), DataFinal = new DateTime(2020,6,30) },
                new Semestre { Titulo = "2022/1", DataInicial = new DateTime(2022,1,1), DataFinal = new DateTime(2022,6,30) },
                new Semestre { Titulo = "2022/2", DataInicial = new DateTime(2022,7,1), DataFinal = new DateTime(2022,12,31) },                
            };
            context.Semestres.AddRange(semestres);
            await context.SaveChangesAsync();

            var horariosGrade = new HorarioGrade[]
            {
                new HorarioGrade { Horario = "2AB" },
                new HorarioGrade { Horario = "2CD" },
                new HorarioGrade { Horario = "2FG" },
                new HorarioGrade { Horario = "2HI" },
                new HorarioGrade { Horario = "2JK" },
                new HorarioGrade { Horario = "2LM" },
                new HorarioGrade { Horario = "2NP" },
                new HorarioGrade { Horario = "3AB" },
                new HorarioGrade { Horario = "3CD" },
                new HorarioGrade { Horario = "3FG" },
                new HorarioGrade { Horario = "3HI" },
                new HorarioGrade { Horario = "3JK" },
                new HorarioGrade { Horario = "3LM" },
                new HorarioGrade { Horario = "3NP" },
                new HorarioGrade { Horario = "4AB" },
                new HorarioGrade { Horario = "4CD" },
                new HorarioGrade { Horario = "4FG" },
                new HorarioGrade { Horario = "4HI" },
                new HorarioGrade { Horario = "4JK" },
                new HorarioGrade { Horario = "4LM" },
                new HorarioGrade { Horario = "4NP" },
                new HorarioGrade { Horario = "5AB" },
                new HorarioGrade { Horario = "5CD" },
                new HorarioGrade { Horario = "5FG" },
                new HorarioGrade { Horario = "5HI" },
                new HorarioGrade { Horario = "5JK" },
                new HorarioGrade { Horario = "5LM" },
                new HorarioGrade { Horario = "5NP" },
                new HorarioGrade { Horario = "6AB" },
                new HorarioGrade { Horario = "6CD" },
                new HorarioGrade { Horario = "6FG" },
                new HorarioGrade { Horario = "6HI" },
                new HorarioGrade { Horario = "6JK" },
                new HorarioGrade { Horario = "6LM" },
                new HorarioGrade { Horario = "6NP" },
            };
            context.HorariosGrade.AddRange(horariosGrade);
            await context.SaveChangesAsync();

            var turmas = new Turma[]
            {
                new Turma { Disciplina = disciplinas[0], DisciplinaId = disciplinas[0].Id, NumeroDeVagas = 60, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[5], horariosGrade[19], horariosGrade[33] } },
                new Turma { Disciplina = disciplinas[1], DisciplinaId = disciplinas[1].Id, NumeroDeVagas = 60, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[6], horariosGrade[13] } },
                new Turma { Disciplina = disciplinas[2], DisciplinaId = disciplinas[2].Id, NumeroDeVagas = 60, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[34] } },
                new Turma { Disciplina = disciplinas[3], DisciplinaId = disciplinas[3].Id, NumeroDeVagas = 60, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[12], horariosGrade[26] } },
                new Turma { Disciplina = disciplinas[4], DisciplinaId = disciplinas[4].Id, NumeroDeVagas = 30, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[13], horariosGrade[27] } },
                new Turma { Disciplina = disciplinas[4], DisciplinaId = disciplinas[4].Id, NumeroDeVagas = 40, Semestre = semestres[1], SemestreId = semestres[1].Id, Horarios = new HorarioGrade[] { horariosGrade[13], horariosGrade[27] } },
                new Turma { Disciplina = disciplinas[2], DisciplinaId = disciplinas[2].Id, NumeroDeVagas = 60, Semestre = semestres[1], SemestreId = semestres[1].Id, Horarios = new HorarioGrade[] { horariosGrade[34] } },
                 new Turma { Disciplina = disciplinas[0], DisciplinaId = disciplinas[0].Id, NumeroDeVagas = 60, Semestre = semestres[1], SemestreId = semestres[1].Id, Horarios = new HorarioGrade[] { horariosGrade[5], horariosGrade[19], horariosGrade[33] } },
            };
            context.Turmas.AddRange(turmas);
            await context.SaveChangesAsync();

            var estudantes = new Estudante[]
            {
                new Estudante { Id = 20103549, Nome = "Cleber", CPF = "000.000.000-01", DigitoVerificador = 0, EstadoEstudanteEnum = EstadoEstudanteEnum.SUSPENSO },
                new Estudante { Id = 20160982, Nome = "Wesley", CPF = "000.000.000-02", DigitoVerificador = 1, EstadoEstudanteEnum = EstadoEstudanteEnum.GRADUADO },
                new Estudante { Id = 20104575, Nome = "Valdomir", CPF = "000.000.000-03", DigitoVerificador = 2, EstadoEstudanteEnum = EstadoEstudanteEnum.ATIVO },
                new Estudante { Id = 20189345, Nome = "Vilmar", CPF = "000.000.000-04", DigitoVerificador = 3, EstadoEstudanteEnum = EstadoEstudanteEnum.ATIVO },
                new Estudante { Id = 20123586, Nome = "Gertrudes", CPF = "000.000.000-05", DigitoVerificador = 4, EstadoEstudanteEnum = EstadoEstudanteEnum.ATIVO },
                new Estudante { Id = 20167846, Nome = "Ernesto", CPF = "000.000.000-06", DigitoVerificador = 5, EstadoEstudanteEnum = EstadoEstudanteEnum.ATIVO },
                new Estudante { Id = 20105486, Nome = "Rafaela", CPF = "000.000.000-07", DigitoVerificador = 6, EstadoEstudanteEnum = EstadoEstudanteEnum.ATIVO },
            };
            context.Estudantes.AddRange(estudantes);
            await context.SaveChangesAsync();

            var matriculaTurmas = new MatriculaTurma[]
            {
                new MatriculaTurma { Nota = 9.5f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id },
                new MatriculaTurma { Nota = 10.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id },
                new MatriculaTurma { Nota = 7.0f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id },
                new MatriculaTurma { Nota = 8.9f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id },
                new MatriculaTurma { Nota = 4.3f, Turma = turmas[4], TurmaId = turmas[4].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id },
                new MatriculaTurma { Nota = 7.1f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id },
                new MatriculaTurma { Nota = 7.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id },
                new MatriculaTurma { Nota = 7.9f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id },
                new MatriculaTurma { Nota = 7.3f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id },
                new MatriculaTurma { Nota = 9.8f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id },
                new MatriculaTurma { Nota = 10.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id },
                new MatriculaTurma { Nota = 10.0f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id },
                new MatriculaTurma { Nota = 9.9f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id },
                new MatriculaTurma { Nota = 10.0f, Turma = turmas[4], TurmaId = turmas[4].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id },
                new MatriculaTurma { Nota = 5.4f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id },
                new MatriculaTurma { Nota = 3.2f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id },
                new MatriculaTurma { Nota = 6.1f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id },
                new MatriculaTurma { Nota = 7.2f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id },
                new MatriculaTurma { Nota = 6.0f, Turma = turmas[4], TurmaId = turmas[4].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id },
                new MatriculaTurma { Nota = 9.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[6], EstudanteId = estudantes[6].Id },
                new MatriculaTurma { Nota = 3.0f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[6], EstudanteId = estudantes[6].Id },
                new MatriculaTurma { Nota = 4.0f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[6], EstudanteId = estudantes[6].Id },
            };
            context.MatriculaTurmas.AddRange(matriculaTurmas);
            await context.SaveChangesAsync();
        }
    }
}
