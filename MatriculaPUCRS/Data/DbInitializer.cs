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

            // Verifica se exitem as roles e adiciona no DB
            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }

            // Verifica se existem coordenadores
            if (!await userManager.GetUsersInRoleAsync(Roles.Coordenador.ToString()).ContinueWith(users => users.Result.Any()))
            {
                var userCoordenador = new ApplicationUser
                {
                    Id = "9b24d507-384c-462f-9616-e2d8e7576548",
                    // EstudanteId = null,
                    UserName = "coordenador@pucrs.com",
                    NormalizedUserName = "COORDENADOR@PUCRS.COM",
                    Email = "coordenador@pucrs.com",
                    NormalizedEmail = "COORDENADOR@PUCRS.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEBR+cMG2CKDLvTiOL1HA7dqwtZFhqg2ck5de/ItQhnIGzOUnHJ9/7+9zJdg3B12eAg==", // 123456
                    SecurityStamp = "MHVNU5SGQJ6WRQHAT5OBFNGYY4I6OKRS",
                    ConcurrencyStamp = "6b4ef5b5-6e57-4f8f-956b-2198ffc576c6",
                    //PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    //LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };

                await userManager.CreateAsync(userCoordenador);
                await userManager.AddToRoleAsync(userCoordenador, Roles.Coordenador.ToString());
            }

            // Verifica se existem os estudantes
            if (!await userManager.GetUsersInRoleAsync(Roles.Estudante.ToString()).ContinueWith(users => users.Result.Any()))
            {
                var userEstudante1 = new ApplicationUser
                {
                    Id = "3af176d5-b6e8-42ab-b12e-2400216a4dca",
                    EstudanteId = 11111111,
                    UserName = "aluno1@pucrs.com",
                    NormalizedUserName = "ALUNO1@PUCRS.COM",
                    Email = "aluno1@pucrs.com",
                    NormalizedEmail = "ALUNO1@PUCRS.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEFt8qzjaNj2UDIwFhhEtUT+Yb1R6Lg35ZwmgpplFkSSVhLapQv3ZiMrhAp+J3SPLOQ==", // 123456
                    SecurityStamp = "SEZVKDWT7GO6JGEOKDEHLGYVEBMP4AZD",
                    ConcurrencyStamp = "173e1d9b-91d4-4b52-833c-32ab893e627d",
                    //PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    //LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };
                await userManager.CreateAsync(userEstudante1);
                await userManager.AddToRoleAsync(userEstudante1, Roles.Estudante.ToString());

                var userEstudante2 = new ApplicationUser
                {
                    Id = "f0151dae-5ef7-45fe-b578-f0c31a7f8382",
                    EstudanteId = 22222222,
                    UserName = "aluno2@pucrs.com",
                    NormalizedUserName = "ALUNO2@PUCRS.COM",
                    Email = "aluno2@pucrs.com",
                    NormalizedEmail = "ALUNO2@PUCRS.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAELqvVQPH5euT6EuO1tf9Iy+GUR/wKrvm0oQKBaY28s92iiEFTdTcdhVb+0FlRgpdrQ==", // 123456
                    SecurityStamp = "NUFHQ3N3MTOIP6V3ZTQAPL2ZGVMAFAA3",
                    ConcurrencyStamp = "0c20be27-cc97-4f48-850f-2843d1194777",
                    //PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    //LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };
                await userManager.CreateAsync(userEstudante2);
                await userManager.AddToRoleAsync(userEstudante2, Roles.Estudante.ToString());
            }

            // Verifica se existem curriculos
            if (context.Curriculos.Any())
            {
                return;   // DB existe
            }

            var curriculos = new Curriculo[]
            {
                new Curriculo { NomeDoCurso = "Sistemas de Informação", Codigo = "4/624", Ativo = true }
            };
            context.Curriculos.AddRange(curriculos);
            await context.SaveChangesAsync();

            var disciplinas = new Disciplina[]
            {
                new Disciplina { Nivel = 1, Codigo = "254PF-04", Nome = "Fundamentos Aplicados a Administração", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 1, Codigo = "4611C-06", Nome = "Fundamentos de Programação", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 90 },
                new Disciplina { Nivel = 1, Codigo = "4636U-04", Nome = "Introdução a Sistemas de Informação", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
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
                new Disciplina { Nivel = 5, Codigo = "4636L-04", Nome = "Gerência de Projetos de TI", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
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
                new Disciplina { Nivel = 4, Codigo = "4647D-04", Nome = "Sistemas Operacionais", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
                new Disciplina { Nivel = 4, Codigo = "46520-04", Nome = "Modelagem e Projeto de Banco de Dados", Curriculos = new Curriculo[] { curriculos[0] }, CargaHoraria = 60 },
            };
            context.Disciplinas.AddRange(disciplinas);
            await context.SaveChangesAsync();

            disciplinas[7].Requisitos = new Disciplina[] { disciplinas[1] };
            disciplinas[8].Requisitos = new Disciplina[] { disciplinas[2] };
            disciplinas[11].Requisitos = new Disciplina[] { disciplinas[5] };
            disciplinas[12].Requisitos = new Disciplina[] { disciplinas[7] };
            disciplinas[13].Requisitos = new Disciplina[] { disciplinas[8] };
            disciplinas[15].Requisitos = new Disciplina[] { disciplinas[3] };
            disciplinas[16].Requisitos = new Disciplina[] { disciplinas[8] };
            disciplinas[17].Requisitos = new Disciplina[] { disciplinas[15] };
            disciplinas[18].Requisitos = new Disciplina[] { disciplinas[7], disciplinas[9], disciplinas[13], disciplinas[16] };
            disciplinas[19].Requisitos = new Disciplina[] { disciplinas[16] };
            disciplinas[47].Requisitos = new Disciplina[] { disciplinas[15] };
            disciplinas[48].Requisitos = new Disciplina[] { disciplinas[11] };
            disciplinas[20].Requisitos = new Disciplina[] { disciplinas[48] };
            disciplinas[22].Requisitos = new Disciplina[] { disciplinas[17] };
            disciplinas[23].Requisitos = new Disciplina[] { disciplinas[48] };
            disciplinas[24].Requisitos = new Disciplina[] { disciplinas[12] };
            disciplinas[25].Requisitos = new Disciplina[] { disciplinas[47] };
            disciplinas[26].Requisitos = new Disciplina[] { disciplinas[14] };
            disciplinas[27].Requisitos = new Disciplina[] { disciplinas[14] };
            disciplinas[28].Requisitos = new Disciplina[] { disciplinas[22] };
            disciplinas[29].Requisitos = new Disciplina[] { disciplinas[14], disciplinas[18], disciplinas[19], disciplinas[21], disciplinas[23] };
            disciplinas[30].Requisitos = new Disciplina[] { disciplinas[5], disciplinas[6] };
            disciplinas[31].Requisitos = new Disciplina[] { disciplinas[23] };
            disciplinas[33].Requisitos = new Disciplina[] { disciplinas[0] };
            disciplinas[35].Requisitos = new Disciplina[] { disciplinas[10] };
            disciplinas[37].Requisitos = new Disciplina[] { disciplinas[12] };
            disciplinas[42].Requisitos = new Disciplina[] { disciplinas[33] };
            disciplinas[45].Requisitos = new Disciplina[] { disciplinas[19] };
            disciplinas[46].Requisitos = new Disciplina[] { disciplinas[37] };
            await context.SaveChangesAsync();

            var semestres = new Semestre[]
            {
                new Semestre { Titulo = "2020/1", DataInicial = new DateTime(2020,1,1), DataFinal = new DateTime(2020,6,30) },
                new Semestre { Titulo = "2020/2", DataInicial = new DateTime(2020,7,1), DataFinal = new DateTime(2020,12,31) },
                new Semestre { Titulo = "2021/1", DataInicial = new DateTime(2021,1,1), DataFinal = new DateTime(2021,6,30) },
                new Semestre { Titulo = "2021/2", DataInicial = new DateTime(2021,7,1), DataFinal = new DateTime(2021,12,31) },
                new Semestre { Titulo = "2022/1", DataInicial = new DateTime(2022,1,1), DataFinal = new DateTime(2022,6,30) },
                new Semestre { Titulo = "2022/2", DataInicial = new DateTime(2022,7,1), DataFinal = new DateTime(2022,12,31) },
                new Semestre { Titulo = "2023/1", DataInicial = new DateTime(2023,1,1), DataFinal = new DateTime(2023,6,30) },
                new Semestre { Titulo = "2023/2", DataInicial = new DateTime(2023,7,1), DataFinal = new DateTime(2023,12,31) },
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
                new Turma { Disciplina = disciplinas[4], DisciplinaId = disciplinas[4].Id, NumeroDeVagas = 40, Semestre = semestres[4], SemestreId = semestres[4].Id, Horarios = new HorarioGrade[] { horariosGrade[13], horariosGrade[27] } },
                new Turma { Disciplina = disciplinas[2], DisciplinaId = disciplinas[2].Id, NumeroDeVagas = 60, Semestre = semestres[4], SemestreId = semestres[4].Id, Horarios = new HorarioGrade[] { horariosGrade[34] } },
                new Turma { Disciplina = disciplinas[0], DisciplinaId = disciplinas[0].Id, NumeroDeVagas = 60, Semestre = semestres[4], SemestreId = semestres[4].Id, Horarios = new HorarioGrade[] { horariosGrade[5], horariosGrade[19], horariosGrade[33] } },
                new Turma { Disciplina = disciplinas[7], DisciplinaId = disciplinas[7].Id, NumeroDeVagas = 45, Semestre = semestres[4], SemestreId = semestres[4].Id, Horarios = new HorarioGrade[] { horariosGrade[5], horariosGrade[19], horariosGrade[33] } },
                new Turma { Disciplina = disciplinas[9], DisciplinaId = disciplinas[9].Id, NumeroDeVagas = 35, Semestre = semestres[4], SemestreId = semestres[4].Id, Horarios = new HorarioGrade[] { horariosGrade[13], horariosGrade[27] } },
                new Turma { Disciplina = disciplinas[9], DisciplinaId = disciplinas[9].Id, NumeroDeVagas = 30, Semestre = semestres[4], SemestreId = semestres[4].Id, Horarios = new HorarioGrade[] { horariosGrade[12], horariosGrade[26] } },
                new Turma { Disciplina = disciplinas[3], DisciplinaId = disciplinas[3].Id, NumeroDeVagas = 25, Semestre = semestres[4], SemestreId = semestres[4].Id, Horarios = new HorarioGrade[] { horariosGrade[5], horariosGrade[19], } },
            };
            context.Turmas.AddRange(turmas);
            await context.SaveChangesAsync();

            var estudantes = new Estudante[]
            {
                // Estudantes com login                
                new Estudante { Id = 11111111, Nome = "Aluno1", CPF = "111.111.111-11", Estado = EstadoEstudanteEnum.ATIVO, Curriculo = curriculos[0] },
                new Estudante { Id = 22222222, Nome = "Aluno2", CPF = "222.222.222-22", Estado = EstadoEstudanteEnum.ATIVO, Curriculo = curriculos[0] },
                // Estudantes sem login
                new Estudante { Id = 20103549, Nome = "Cleber", CPF = "000.000.000-01", Estado = EstadoEstudanteEnum.SUSPENSO, Curriculo = curriculos[0] },
                new Estudante { Id = 20160982, Nome = "Wesley", CPF = "000.000.000-02", Estado = EstadoEstudanteEnum.GRADUADO, Curriculo = curriculos[0] },
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
                new MatriculaTurma { Nota = 9.5f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 10.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 7.0f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 8.9f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 4.3f, Turma = turmas[4], TurmaId = turmas[4].Id, Estudante = estudantes[2], EstudanteId = estudantes[2].Id, Estado = EstadoMatriculaTurmaEnum.REPROVADO },
                new MatriculaTurma { Nota = 7.1f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 7.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 7.9f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 7.3f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[3], EstudanteId = estudantes[3].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 9.8f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 10.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 10.0f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 9.9f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 10.0f, Turma = turmas[4], TurmaId = turmas[4].Id, Estudante = estudantes[4], EstudanteId = estudantes[4].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 5.4f, Turma = turmas[0], TurmaId = turmas[0].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 3.2f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id, Estado = EstadoMatriculaTurmaEnum.REPROVADO },
                new MatriculaTurma { Nota = 6.1f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 7.2f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 6.0f, Turma = turmas[4], TurmaId = turmas[4].Id, Estudante = estudantes[5], EstudanteId = estudantes[5].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 9.0f, Turma = turmas[1], TurmaId = turmas[1].Id, Estudante = estudantes[6], EstudanteId = estudantes[6].Id, Estado = EstadoMatriculaTurmaEnum.APROVADO },
                new MatriculaTurma { Nota = 3.0f, Turma = turmas[2], TurmaId = turmas[2].Id, Estudante = estudantes[6], EstudanteId = estudantes[6].Id, Estado = EstadoMatriculaTurmaEnum.REPROVADO },
                new MatriculaTurma { Nota = 4.0f, Turma = turmas[3], TurmaId = turmas[3].Id, Estudante = estudantes[6], EstudanteId = estudantes[6].Id, Estado = EstadoMatriculaTurmaEnum.REPROVADO },
            };
            context.MatriculaTurmas.AddRange(matriculaTurmas);
            await context.SaveChangesAsync();
        }
    }
}
