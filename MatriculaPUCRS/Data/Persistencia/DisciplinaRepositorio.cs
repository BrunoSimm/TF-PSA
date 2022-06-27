﻿using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class DisciplinaRepositorio : CrudEF<Disciplina, MatriculaContext>, IDisciplinaRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public DisciplinaRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }

        public IEnumerable<Disciplina> GetDisciplinasWithTurmasFromSemestre(long estudanteId, long semestreId)
        {
            Estudante estudante = _matriculaContext.Estudantes
                .Include(e => e.Matriculas)
                    .ThenInclude(mt => mt.Turma)
                    .ThenInclude(t => t.Disciplina)
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == estudanteId);

            if (estudante is null)
            {
                return null;
            }

            var disciplinasCursadas = estudante.Matriculas
                .Where(mt => (mt.Estado == EstadoMatriculaTurmaEnum.APROVADO) ^ (mt.Estado == EstadoMatriculaTurmaEnum.CURSANDO))
                .Select(mt => mt.Turma.Disciplina)
                .Distinct();

            var disciplinasAprovado = estudante.Matriculas
                .Where(mt => mt.Aprovado)
                .Select(mt => mt.Turma.Disciplina);

            var disciplinasDisponiveis = _matriculaContext.Disciplinas
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Semestre)
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Horarios)
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Matriculas)
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Semestre)
                .Include(d => d.Requisitos)
                .Where(d => d.Turmas.Any(t => t.SemestreId == semestreId))
                .AsNoTracking()
                .AsEnumerable()
                .Where(d => !disciplinasCursadas.Contains(d))
                .Where(d => d.Requisitos.All(dr => disciplinasAprovado.Contains(dr)));
            
            return disciplinasDisponiveis;
        }

        public Task<Disciplina> GetDisciplinaByIdWithMatriculasAndSemestre(long id)
        {
            return _matriculaContext.Disciplinas
                .Include(d => d.Turmas).ThenInclude(t => t.Semestre)
                .Include(d => d.Turmas).ThenInclude(t => t.Horarios)
                .Include(d => d.Turmas).ThenInclude(t => t.Matriculas)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public Task<Curriculo> GetDisciplinasFromCurriculoId(long id)
        {
            return _matriculaContext.Curriculos.Include(c => c.Disciplinas).FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Disciplina> GetDisciplinasIQueryable()
        {
            return _matriculaContext.Disciplinas;
        }
    }
}
