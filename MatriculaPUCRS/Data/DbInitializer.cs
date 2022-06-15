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
                new Curriculo { NomeDoCurso = "Sistemas de Informação", Codigo = "4/624", Ativo = true }
            };
            context.Curriculos.AddRange(curriculos);
            await context.SaveChangesAsync();

            var disciplinas = new Disciplina[]
            {
                new Disciplina { Nivel = 1, Codigo = "254PF-04", Nome = "Fundamentos Aplicados à Administração", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 1, Codigo = "4611C-06", Nome = "Fundamentos de Programação", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 90 },
                new Disciplina { Nivel = 1, Codigo = "4636U-04", Nome = "Introdução à Sistemas de Informação", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 1, Codigo = "4637P-02", Nome = "Fundamentos da Computação", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 1, Codigo = "95300-04", Nome = "Cálculo I", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 2, Codigo = "4115L-02", Nome = "Matemática Discreta (SI)", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 2, Codigo = "4611E-04", Nome = "Lógica para Computação", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 2, Codigo = "4611F-04", Nome = "Programação Orientada a Objetos", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 2, Codigo = "4636F-02", Nome = "Engenharia de Software", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 2, Codigo = "4636X-04", Nome = "Modelagem de Negócio", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 2, Codigo = "95304-04", Nome = "Probabilidade e Estatística", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 3, Codigo = "4611D-02", Nome = "Laboratório de Banco de Dados", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 3, Codigo = "4636H-04", Nome = "Fundamentos de Desenvolvimento de Software", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 3, Codigo = "4636T-04", Nome = "Interação Humano-Computador", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 3, Codigo = "4636Z-04", Nome = "Planejamento e Gestão Estratégica de TI", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 3, Codigo = "4637N-02", Nome = "Arquitetura de Computadores", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 3, Codigo = "46506-04", Nome = "Engenharia de Requisitos", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 4, Codigo = "4636K-04", Nome = "Fundamentos de Redes de Computadores", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 4, Codigo = "4636P-04", Nome = "Disicplina Integradora", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 4, Codigo = "4637C-04", Nome = "Projeto e Desenvolvimento de Software", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 5, Codigo = "4611H-02", Nome = "Sistema de Gerenciamento de Banco de Dados", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 5, Codigo = "4636L-04", Nome = "Gerência de Projeos de TI", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 5, Codigo = "4636M-02", Nome = "Gerência de Redes de Computadores", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 5, Codigo = "4636R-04", Nome = "Inteligência de Negócio", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 5, Codigo = "4637B-04", Nome = "Programação de Software Aplicado", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 5, Codigo = "4637G-04", Nome = "Sistemas Distribuídos", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 6, Codigo = "254AU-04", Nome = "Governança Estratégica em TI", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 6, Codigo = "4636B-04", Nome = "Arquitetura Organizacional Aplicada a SI", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 6, Codigo = "4636N-04", Nome = "Infraestrutura de Tecnologia da Informação", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 6, Codigo = "4636Q-04", Nome = "Disciplina Integradora II", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 6, Codigo = "4637J-02", Nome = "Teoria da Computação", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 6, Codigo = "4637K-02", Nome = "Tópicos Avançados em Gestão de Dados", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 7, Codigo = "11521-04", Nome = "Humanismo e Cultura Religiosa", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 7, Codigo = "2540L-04", Nome = "Empreendimentos Empresariais", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 7, Codigo = "4613Y-08", Nome = "Disciplina Eletiva", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 120 },
                new Disciplina { Nivel = 7, Codigo = "4636D-02", Nome = "Avaliação e Desempenho de Sistemas", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 7, Codigo = "4637A-02", Nome = "Prática Profissional (360 Horas)", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 360 },
                new Disciplina { Nivel = 7, Codigo = "4637E-02", Nome = "Qualidade de Produto", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 7, Codigo = "4637F-04", Nome = "Sistemas de Informação Integrados", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 7, Codigo = "4637L-04", Nome = "Trabalho de Conclusão I", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 8, Codigo = "1501A-04", Nome = "Ética e Cidadania", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 8, Codigo = "24135-02", Nome = "Legislação em Informática", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 8, Codigo = "254PG-02", Nome = "Comportamento Organizacional", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 30 },
                new Disciplina { Nivel = 8, Codigo = "4612D-00", Nome = "Atividades Complementares", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 120 },
                new Disciplina { Nivel = 8, Codigo = "4636C-04", Nome = "Auditoria e Segurançaa de Sistemas de Informação", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 8, Codigo = "4637D-04", Nome = "Qualidade de Processo", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 8, Codigo = "4637M-04", Nome = "Trabalho de Conclusão II", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
            };
            context.Disciplinas.AddRange(disciplinas);
            await context.SaveChangesAsync();

            var requisitos = new Requisito[]
            {
                new Requisito { Disciplina = disciplinas[0], DisciplinaId = disciplinas[0].Id, DisciplinaOrigem = disciplinas[9], DisciplinaOrigemId = disciplinas[9].Id, TipoRequisito = TipoRequisitoEnum.PRE_REQUISITO },
                new Requisito { Disciplina = disciplinas[1], DisciplinaId = disciplinas[1].Id, DisciplinaOrigem = disciplinas[7], DisciplinaOrigemId = disciplinas[7].Id, TipoRequisito = TipoRequisitoEnum.PRE_REQUISITO }
            };
            context.Requisitos.AddRange(requisitos);
            await context.SaveChangesAsync();

            var semestres = new Semestre[]
            {
                new Semestre { Titulo = "2020/1", DataInicial = new DateTime(2020,1,1), DataFinal = new DateTime(2020,6,30) },
                new Semestre { Titulo = "2020/2", DataInicial = new DateTime(2020,7,1), DataFinal = new DateTime(2020,12,31) },
                new Semestre { Titulo = "2021/1", DataInicial = new DateTime(2021,1,1), DataFinal = new DateTime(2020,6,30) },
                new Semestre { Titulo = "2021/2", DataInicial = new DateTime(2021,7,1), DataFinal = new DateTime(2020,12,31) },
                new Semestre { Titulo = "2022/1", DataInicial = new DateTime(2022,1,1), DataFinal = new DateTime(2020,6,30) },
                new Semestre { Titulo = "2022/2", DataInicial = new DateTime(2022,7,1), DataFinal = new DateTime(2020,12,31) },
                new Semestre { Titulo = "2023/1", DataInicial = new DateTime(2023,1,1), DataFinal = new DateTime(2022,6,30) },
                new Semestre { Titulo = "2023/2", DataInicial = new DateTime(2023,7,1), DataFinal = new DateTime(2022,12,31) },
            };
            context.Semestres.AddRange(semestres);
            await context.SaveChangesAsync();

            var horariosGrade = new HorarioGrade[]
            {
                new HorarioGrade { Horario = "2AB" , HorarioInicial = "08:00" , HorarioFinal = "09:30" },
                new HorarioGrade { Horario = "2CD" , HorarioInicial = "09:45" , HorarioFinal = "11:15" },
                new HorarioGrade { Horario = "2FG" , HorarioInicial = "14:00" , HorarioFinal = "15:30" },
                new HorarioGrade { Horario = "2HI" , HorarioInicial = "15:45" , HorarioFinal = "17:15" },
                new HorarioGrade { Horario = "2JK" , HorarioInicial = "17:30" , HorarioFinal = "19:00" },
                new HorarioGrade { Horario = "2LM" , HorarioInicial = "19:15" , HorarioFinal = "20:45" },
                new HorarioGrade { Horario = "2NP" , HorarioInicial = "21:00" , HorarioFinal = "22:30" },
                new HorarioGrade { Horario = "3AB" , HorarioInicial = "08:00" , HorarioFinal = "09:30" },
                new HorarioGrade { Horario = "3CD" , HorarioInicial = "09:45" , HorarioFinal = "11:15" },
                new HorarioGrade { Horario = "3FG" , HorarioInicial = "14:00" , HorarioFinal = "15:30" },
                new HorarioGrade { Horario = "3HI" , HorarioInicial = "15:45" , HorarioFinal = "17:15" },
                new HorarioGrade { Horario = "3JK" , HorarioInicial = "17:30" , HorarioFinal = "19:00" },
                new HorarioGrade { Horario = "3LM" , HorarioInicial = "19:15" , HorarioFinal = "20:45" },
                new HorarioGrade { Horario = "3NP" , HorarioInicial = "21:00" , HorarioFinal = "22:30" },
                new HorarioGrade { Horario = "4AB" , HorarioInicial = "08:00" , HorarioFinal = "09:30" },
                new HorarioGrade { Horario = "4CD" , HorarioInicial = "09:45" , HorarioFinal = "11:15" },
                new HorarioGrade { Horario = "4FG" , HorarioInicial = "14:00" , HorarioFinal = "15:30" },
                new HorarioGrade { Horario = "4HI" , HorarioInicial = "15:45" , HorarioFinal = "17:15" },
                new HorarioGrade { Horario = "4JK" , HorarioInicial = "17:30" , HorarioFinal = "19:00" },
                new HorarioGrade { Horario = "4LM" , HorarioInicial = "19:15" , HorarioFinal = "20:45" },
                new HorarioGrade { Horario = "4NP" , HorarioInicial = "21:00" , HorarioFinal = "22:30" },
                new HorarioGrade { Horario = "5AB" , HorarioInicial = "08:00" , HorarioFinal = "09:30" },
                new HorarioGrade { Horario = "5CD" , HorarioInicial = "09:45" , HorarioFinal = "11:15" },
                new HorarioGrade { Horario = "5FG" , HorarioInicial = "14:00" , HorarioFinal = "15:30" },
                new HorarioGrade { Horario = "5HI" , HorarioInicial = "15:45" , HorarioFinal = "17:15" },
                new HorarioGrade { Horario = "5JK" , HorarioInicial = "17:30" , HorarioFinal = "19:00" },
                new HorarioGrade { Horario = "5LM" , HorarioInicial = "19:15" , HorarioFinal = "20:45" },
                new HorarioGrade { Horario = "5NP" , HorarioInicial = "21:00" , HorarioFinal = "22:30" },
                new HorarioGrade { Horario = "6AB" , HorarioInicial = "08:00" , HorarioFinal = "09:30" },
                new HorarioGrade { Horario = "6CD" , HorarioInicial = "09:45" , HorarioFinal = "11:15" },
                new HorarioGrade { Horario = "6FG" , HorarioInicial = "14:00" , HorarioFinal = "15:30" },
                new HorarioGrade { Horario = "6HI" , HorarioInicial = "15:45" , HorarioFinal = "17:15" },
                new HorarioGrade { Horario = "6JK" , HorarioInicial = "17:30" , HorarioFinal = "19:00" },
                new HorarioGrade { Horario = "6LM" , HorarioInicial = "19:15" , HorarioFinal = "20:45" },
                new HorarioGrade { Horario = "6NP" , HorarioInicial = "21:00" , HorarioFinal = "22:30" },
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
                new Turma { Disciplina = disciplinas[4], DisciplinaId = disciplinas[4].Id, NumeroDeVagas = 40, Semestre = semestres[4], SemestreId = semestres[1].Id, Horarios = new HorarioGrade[] { horariosGrade[13], horariosGrade[27] } },
                new Turma { Disciplina = disciplinas[2], DisciplinaId = disciplinas[2].Id, NumeroDeVagas = 60, Semestre = semestres[4], SemestreId = semestres[1].Id, Horarios = new HorarioGrade[] { horariosGrade[34] } },
                new Turma { Disciplina = disciplinas[0], DisciplinaId = disciplinas[0].Id, NumeroDeVagas = 60, Semestre = semestres[4], SemestreId = semestres[1].Id, Horarios = new HorarioGrade[] { horariosGrade[5], horariosGrade[19], horariosGrade[33] } },
                new Turma { Disciplina = disciplinas[7], DisciplinaId = disciplinas[7].Id, NumeroDeVagas = 45, Semestre = semestres[4], SemestreId = semestres[1].Id, Horarios = new HorarioGrade[] { horariosGrade[5], horariosGrade[19], horariosGrade[33] } },
                new Turma { Disciplina = disciplinas[9], DisciplinaId = disciplinas[9].Id, NumeroDeVagas = 35, Semestre = semestres[4], SemestreId = semestres[1].Id, Horarios = new HorarioGrade[] { horariosGrade[13], horariosGrade[27] } },
            };
            context.Turmas.AddRange(turmas);
            await context.SaveChangesAsync();

            var estudantes = new Estudante[]
            {
                new Estudante { Id = 20103549, Nome = "Cleber", CPF = "000.000.000-01", Estado = EstadoEstudanteEnum.SUSPENSO, Curriculo = curriculos[0] },
                new Estudante { Id = 20160982, Nome = "Wesley", CPF = "000.000.000-02", Estado = EstadoEstudanteEnum.GRADUADO },
                new Estudante { Id = 20104575, Nome = "Valdomir", CPF = "000.000.000-03", Estado = EstadoEstudanteEnum.ATIVO, Curriculo = curriculos[0] },
                new Estudante { Id = 20189345, Nome = "Vilmar", CPF = "000.000.000-04", Estado = EstadoEstudanteEnum.ATIVO, Curriculo = curriculos[0] },
                new Estudante { Id = 20123586, Nome = "Gertrudes", CPF = "000.000.000-05", Estado = EstadoEstudanteEnum.ATIVO, Curriculo = curriculos[0] },
                new Estudante { Id = 20167846, Nome = "Ernesto", CPF = "000.000.000-06", Estado = EstadoEstudanteEnum.ATIVO, Curriculo = curriculos[0] },
                new Estudante { Id = 20105486, Nome = "Rafaela", CPF = "000.000.000-07", Estado = EstadoEstudanteEnum.ATIVO, Curriculo = curriculos[0] },
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
