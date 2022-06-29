using Entidades.Modelos;
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

        public IEnumerable<IGrouping<Disciplina, Turma>> GetDisciplinasWithTurmasFromSemestre(long estudanteId, long semestreId)
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

            var disciplinasDisponiveis = _matriculaContext.Turmas
                .Include(t => t.Semestre)
                .Include(t => t.Horarios)
                .Include(t => t.Matriculas)
                .Include(t => t.Disciplina)
                    .ThenInclude(d => d.Requisitos)
                .Where(t => t.SemestreId == semestreId)
                .AsNoTracking()
                .AsEnumerable()
                .GroupBy(t => t.Disciplina)
                .Where(d => !disciplinasCursadas.Contains(d.Key))
                .Where(d => d.Key.Requisitos.All(dr => disciplinasAprovado.Contains(dr)));

            return disciplinasDisponiveis;
        }

        public Task<Disciplina> GetDisciplinaByIdWithMatriculasAndSemestre(long id)
        {
            return _matriculaContext.Disciplinas
                .Include(d => d.Curriculos)
                .Include(d => d.Turmas).ThenInclude(t => t.Semestre)
                .Include(d => d.Turmas).ThenInclude(t => t.Horarios)
                .Include(d => d.Turmas).ThenInclude(t => t.Matriculas)
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Matriculas).ThenInclude(mt => mt.Estudante).ThenInclude(e => e.Curriculo)
                .Include(d => d.Requisitos)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public Task<Curriculo> GetDisciplinasFromCurriculoId(long id)
        {
            return _matriculaContext.Curriculos
                .Include(c => c.Disciplinas)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Disciplina> GetDisciplinasIQueryable()
        {
            return _matriculaContext.Disciplinas;
        }

        public Task<Curriculo> GetDisciplinasWithTurmasFromSemesterId(long id)
        {
            return _matriculaContext.Curriculos
                .Include(c => c.Disciplinas)
                    .ThenInclude(d => d.Turmas)
                    .ThenInclude(t => t.Matriculas)
                    .ThenInclude(mt => mt.Estudante)
                .Include(c => c.Disciplinas)
                    .ThenInclude(d => d.Turmas)
                    .ThenInclude(t => t.Semestre)
                .FirstOrDefaultAsync(c => c.Disciplinas.Any(d => d.Turmas.Any(t => t.SemestreId == id)));
        }
    }
}
