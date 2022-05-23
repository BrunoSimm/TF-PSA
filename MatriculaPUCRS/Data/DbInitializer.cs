using Entidades.Modelos;
using Infraestrutura.Data;
using System;
using System.Linq;

namespace MatriculaPUCRS.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MatriculaContext context)
        {
            context.Database.EnsureCreated();
            
            // Look for any students.
            if (context.Curriculos.Any())
            {
                return;   // DB has been seeded
            }

            var disciplinas = new Disciplina[]
            {
                new Disciplina { Codigo = "4611C-06", Nome = "Fundamentos de Programação", },
                new Disciplina { Codigo = "95300-04", Nome = "Cálculo 1", },
                new Disciplina { Codigo = "4647P-02", Nome = "Fundamentos da Computação", },
                new Disciplina { Codigo = "4636U-04", Nome = "Introdução a Sistemas de Informação", },
                new Disciplina { Codigo = "254PF-04", Nome = "Fundamentos Aplicados a Administração", }
            };

            var curriculos = new Curriculo[]
            {
                new Curriculo { Codigo = "4/624", Ativo = true, Disciplinas= disciplinas }
            };

            foreach(var disciplina in disciplinas)
            {
                disciplina.Curriculos = new Curriculo[] { curriculos[0] };
            }

            var semestres = new Semestre[]
            {
                new Semestre { Titulo = "2020/1", DataInicial = new DateTime(2020,1,1), DataFinal = new DateTime(2020,6,30)}
            };

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

            var turmas = new Turma[]
            {
                new Turma { Disciplina = disciplinas[0], DisciplinaId = disciplinas[0].Id, NumeroDeVagas = 60, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[5], horariosGrade[19], horariosGrade[33] } },
                new Turma { Disciplina = disciplinas[1], DisciplinaId = disciplinas[1].Id, NumeroDeVagas = 60, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[6], horariosGrade[13] } },
                new Turma { Disciplina = disciplinas[2], DisciplinaId = disciplinas[2].Id, NumeroDeVagas = 60, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[34] } },
                new Turma { Disciplina = disciplinas[3], DisciplinaId = disciplinas[3].Id, NumeroDeVagas = 60, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[12], horariosGrade[26] } },
                new Turma { Disciplina = disciplinas[4], DisciplinaId = disciplinas[4].Id, NumeroDeVagas = 30, Semestre = semestres[0], SemestreId = semestres[0].Id, Horarios = new HorarioGrade[] { horariosGrade[13], horariosGrade[27] },  },
            };
            semestres[0].Turmas = turmas;

            disciplinas[0].Turmas = new Turma[] { turmas[0] }; 
            disciplinas[1].Turmas = new Turma[] { turmas[1] }; 
            disciplinas[2].Turmas = new Turma[] { turmas[2] }; 
            disciplinas[3].Turmas = new Turma[] { turmas[3] }; 
            disciplinas[4].Turmas = new Turma[] { turmas[4] };

            horariosGrade[5].Turmas = new Turma[] { turmas[0] };
            horariosGrade[19].Turmas = new Turma[] { turmas[0] };
            horariosGrade[33].Turmas = new Turma[] { turmas[0] };
            horariosGrade[6].Turmas = new Turma[] { turmas[1] };
            horariosGrade[13].Turmas = new Turma[] { turmas[1] };
            horariosGrade[34].Turmas = new Turma[] { turmas[2] };
            horariosGrade[12].Turmas = new Turma[] { turmas[3] };
            horariosGrade[26].Turmas = new Turma[] { turmas[3] };
            horariosGrade[13].Turmas = new Turma[] { turmas[4] };
            horariosGrade[27].Turmas = new Turma[] { turmas[4] };

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

            var matriculaTurmas = new MatriculaTurma[]
            {
                new MatriculaTurma { Aprovado = true, Nota = 9.5f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id },
                new MatriculaTurma { Aprovado = true, Nota = 10.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 7.0f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 8.9f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id  },
                new MatriculaTurma { Aprovado = false, Nota = 4.3f, Turma = turmas[4], TurmaId = turmas[4].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 7.1f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 7.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 7.9f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 7.3f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 9.8f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 10.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 10.0f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 9.9f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 10.0f, Turma = turmas[4], TurmaId = turmas[4].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 5.4f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id  },
                new MatriculaTurma { Aprovado = false, Nota = 3.2f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 6.1f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 7.2f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id  },
                new MatriculaTurma { Aprovado = false, Nota = 6.0f, Turma = turmas[4], TurmaId = turmas[4].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id  },
                new MatriculaTurma { Aprovado = true, Nota = 9.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[6], EstudanteId = estudantes[6].Id  },
                new MatriculaTurma { Aprovado = false, Nota = 3.0f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[6], EstudanteId = estudantes[6].Id  },
                new MatriculaTurma { Aprovado = false, Nota = 4.0f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[6], EstudanteId = estudantes[6].Id  },
            };

            estudantes[2].Matriculas = new MatriculaTurma[] { matriculaTurmas[0], matriculaTurmas[1], matriculaTurmas[2], matriculaTurmas[3], matriculaTurmas[4] };
            estudantes[3].Matriculas = new MatriculaTurma[] { matriculaTurmas[5], matriculaTurmas[6], matriculaTurmas[7], matriculaTurmas[8] };
            estudantes[4].Matriculas = new MatriculaTurma[] { matriculaTurmas[9], matriculaTurmas[10], matriculaTurmas[11], matriculaTurmas[12], matriculaTurmas[13] };
            estudantes[5].Matriculas = new MatriculaTurma[] { matriculaTurmas[14], matriculaTurmas[15], matriculaTurmas[16], matriculaTurmas[17], matriculaTurmas[18] };
            estudantes[6].Matriculas = new MatriculaTurma[] { matriculaTurmas[19], matriculaTurmas[20], matriculaTurmas[21] };

            turmas[0].Matriculas = new MatriculaTurma[] { matriculaTurmas[0], matriculaTurmas[5], matriculaTurmas[9], matriculaTurmas[14] };
            turmas[1].Matriculas = new MatriculaTurma[] { matriculaTurmas[1], matriculaTurmas[6], matriculaTurmas[10], matriculaTurmas[15], matriculaTurmas[19] };
            turmas[2].Matriculas = new MatriculaTurma[] { matriculaTurmas[2], matriculaTurmas[7], matriculaTurmas[11], matriculaTurmas[16], matriculaTurmas[20] };
            turmas[3].Matriculas = new MatriculaTurma[] { matriculaTurmas[3], matriculaTurmas[8], matriculaTurmas[12], matriculaTurmas[17], matriculaTurmas[21] };
            turmas[4].Matriculas = new MatriculaTurma[] { matriculaTurmas[4], matriculaTurmas[13], matriculaTurmas[18] };

            context.Curriculos.AddRange(curriculos);
            context.Disciplinas.AddRange(disciplinas);
            context.Semestres.AddRange(semestres); 
            context.HorariosGrade.AddRange(horariosGrade);
            context.Turmas.AddRange(turmas);
            context.Estudantes.AddRange(estudantes);
            context.MatriculaTurmas.AddRange(matriculaTurmas);
            context.SaveChanges();
        }
    }
}
