-- Curriculos
INSERT INTO Curriculos (Codigo, Ativo)
VALUES ('4/624', 1);

-- Disciplinas
INSERT INTO Disciplinas (Codigo, Nome)
VALUES ('4611C-06', 'Fundamentos de Programação');
INSERT INTO Disciplinas (Codigo, Nome)
VALUES ('95300-04', 'Cálculo 1');
INSERT INTO Disciplinas (Codigo, Nome)
VALUES ('4647P-02', 'Fundamentos da Computação');
INSERT INTO Disciplinas (Codigo, Nome)
VALUES ('4636U-04', 'Introdução a Sistemas de Informação');
INSERT INTO Disciplinas (Codigo, Nome)
VALUES ('254PF-04', 'Fundamentos Aplicados a Administração');

-- CurriculoDisciplina
INSERT INTO CurriculoDisciplina (CurriculosId, DisciplinasId)
VALUES (1,1);
INSERT INTO CurriculoDisciplina (CurriculosId, DisciplinasId)
VALUES (1,2);
INSERT INTO CurriculoDisciplina (CurriculosId, DisciplinasId)
VALUES (1,3);
INSERT INTO CurriculoDisciplina (CurriculosId, DisciplinasId)
VALUES (1,4);
INSERT INTO CurriculoDisciplina (CurriculosId, DisciplinasId)
VALUES (1,5);

-- Semestres
INSERT INTO Semestres (Titulo, DataInicial, DataFinal)
VALUES ('2020/1', CONVERT(DATETIME,'01/01/2020',103), CONVERT(DATETIME,'30/06/2020',103));

-- Turmas
INSERT INTO Turma (NumeroDeVagas, DisciplinaId, SemestreId)
VALUES (60, 1, 1);
INSERT INTO Turma (NumeroDeVagas, DisciplinaId, SemestreId)
VALUES (60, 2, 1);
INSERT INTO Turma (NumeroDeVagas, DisciplinaId, SemestreId)
VALUES (60, 3, 1);
INSERT INTO Turma (NumeroDeVagas, DisciplinaId, SemestreId)
VALUES (60, 4, 1);
INSERT INTO Turma (NumeroDeVagas, DisciplinaId, SemestreId)
VALUES (30, 5, 1);

-- Estudantes
INSERT INTO Estudantes (Id, Nome, CPF, DigitoVerificador, EstadoEstudanteEnum)
VALUES (20103549, 'Cleber', '000.000.000-01', 0, 'SUSPENSO');
INSERT INTO Estudantes (Id, Nome, CPF, DigitoVerificador, EstadoEstudanteEnum)
VALUES (20160982, 'Wesley', '000.000.000-02', 1, 'GRADUADO');
INSERT INTO Estudantes (Id, Nome, CPF, DigitoVerificador, EstadoEstudanteEnum)
VALUES (20104575, 'Valdomir', '000.000.000-03', 2, 'ATIVO');
INSERT INTO Estudantes (Id, Nome, CPF, DigitoVerificador, EstadoEstudanteEnum)
VALUES (20189345, 'Vilmar', '000.000.000-04', 3, 'ATIVO');
INSERT INTO Estudantes (Id, Nome, CPF, DigitoVerificador, EstadoEstudanteEnum)
VALUES (20123586, 'Gertrudes', '000.000.000-05', 4, 'ATIVO');
INSERT INTO Estudantes (Id, Nome, CPF, DigitoVerificador, EstadoEstudanteEnum)
VALUES (20167846, 'Ernesto', '000.000.000-06', 5, 'ATIVO');
INSERT INTO Estudantes (Id, Nome, CPF, DigitoVerificador, EstadoEstudanteEnum)
VALUES (20105486, 'Rafaela', '000.000.000-07', 6, 'ATIVO');

-- MatriculaTurmas
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 9.5, 1, 20104575);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 9.5, 2, 20104575);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 9.5, 3, 20104575);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 9.5, 4, 20104575);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 9.5, 5, 20104575);

INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 7.1, 1, 20189345);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 7.0, 2, 20189345);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 7.9, 3, 20189345);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 7.3, 4, 20189345);
 
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 9.8, 1, 20123586);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 10.0, 2, 20123586);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 10.0, 3, 20123586);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 9.9, 4, 20123586);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 10.0, 5, 20123586);

INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 5.4, 1, 20167846);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (0, 3.2, 2, 20167846);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 6.1, 3, 20167846);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 7.2, 4, 20167846);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 6.0, 5, 20167846);

INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (1, 9.0, 1, 20105486);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (0, 3.0, 2, 20105486);
INSERT INTO MatriculaTurmas (Aprovado, Nota, TurmaId, EstudanteId)
VALUES (0, 4.0, 3, 20105486);

-- HorariosGrade
INSERT INTO HorariosGrade (Horario)
VALUES ('2AB');
INSERT INTO HorariosGrade (Horario)
VALUES ('2CD');
INSERT INTO HorariosGrade (Horario)
VALUES ('2FG');
INSERT INTO HorariosGrade (Horario)
VALUES ('2HI');
INSERT INTO HorariosGrade (Horario)
VALUES ('2JK');
INSERT INTO HorariosGrade (Horario)
VALUES ('2LM');
INSERT INTO HorariosGrade (Horario)
VALUES ('2NP');
INSERT INTO HorariosGrade (Horario)
VALUES ('3AB');
INSERT INTO HorariosGrade (Horario)
VALUES ('3CD');
INSERT INTO HorariosGrade (Horario)
VALUES ('3FG');
INSERT INTO HorariosGrade (Horario)
VALUES ('3HI');
INSERT INTO HorariosGrade (Horario)
VALUES ('3JK');
INSERT INTO HorariosGrade (Horario)
VALUES ('3LM');
INSERT INTO HorariosGrade (Horario)
VALUES ('3NP');
INSERT INTO HorariosGrade (Horario)
VALUES ('4AB');
INSERT INTO HorariosGrade (Horario)
VALUES ('4CD');
INSERT INTO HorariosGrade (Horario)
VALUES ('4FG');
INSERT INTO HorariosGrade (Horario)
VALUES ('4HI');
INSERT INTO HorariosGrade (Horario)
VALUES ('4JK');
INSERT INTO HorariosGrade (Horario)
VALUES ('4LM');
INSERT INTO HorariosGrade (Horario)
VALUES ('4NP');
INSERT INTO HorariosGrade (Horario)
VALUES ('5AB');
INSERT INTO HorariosGrade (Horario)
VALUES ('5CD');
INSERT INTO HorariosGrade (Horario)
VALUES ('5FG');
INSERT INTO HorariosGrade (Horario)
VALUES ('5HI');
INSERT INTO HorariosGrade (Horario)
VALUES ('5JK');
INSERT INTO HorariosGrade (Horario)
VALUES ('5LM');
INSERT INTO HorariosGrade (Horario)
VALUES ('5NP');
INSERT INTO HorariosGrade (Horario)
VALUES ('6AB');
INSERT INTO HorariosGrade (Horario)
VALUES ('6CD');
INSERT INTO HorariosGrade (Horario)
VALUES ('6FG');
INSERT INTO HorariosGrade (Horario)
VALUES ('6HI');
INSERT INTO HorariosGrade (Horario)
VALUES ('6JK');
INSERT INTO HorariosGrade (Horario)
VALUES ('6LM');
INSERT INTO HorariosGrade (Horario)
VALUES ('6NP');

-- HorariosGradeTurma
INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (6,1);
INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (20,1);
INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (34,1);

INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (7,2);
INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (14,2);

INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (35,3);

INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (13,4);
INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (27,4);

INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (21,5);
INSERT INTO HorariosGradeTurma (HorarioId, TurmasId)
VALUES (28,5);