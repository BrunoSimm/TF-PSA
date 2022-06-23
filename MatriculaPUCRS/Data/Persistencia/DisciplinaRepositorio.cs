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

        public Task<List<Disciplina>> GetDisciplinasFromCurrentSemester()
        {
            return _matriculaContext.Turmas
                .Join(
                    _matriculaContext.Disciplinas,
                    turma => turma.DisciplinaId,
                    disc => disc.Id,
                    (turma, disc) => new { turma, disc })
                .Join(
                    _matriculaContext.Semestres,
                    turmaDisc => turmaDisc.turma.SemestreId,
                    semestre => semestre.Id,
                    (turmaDisc, semestre) => new { turmaDisc, semestre})
                .Where(result => DateTime.Now >= result.semestre.DataInicial && DateTime.Now <= result.semestre.DataFinal)
                .Select(result => result.turmaDisc.disc)
                .Include(d => d.Turmas).ThenInclude(t => t.Semestre)
                .Include(d => d.Turmas).ThenInclude(t => t.Horarios)
                .Include(d => d.Turmas).ThenInclude(t => t.Matriculas)
                .ToListAsync();
        }

        public Task<List<Disciplina>> GetDisciplinasFromSemestre(long estudanteId, long semestreId)
        {
            var disciplinasComTurmas = _matriculaContext.Disciplinas
                .Include(d => d.Turmas).ThenInclude(t => t.Semestre)
                .Include(d => d.Turmas).ThenInclude(t => t.Horarios)
                .Include(d => d.Turmas).ThenInclude(t => t.Matriculas)
                .Where(d => d.Turmas.Any(t => t.SemestreId == semestreId));

            var tumasNoSemestre = _matriculaContext.Turmas
                .Include(t => t.Semestre)
                .Include(t => t.Horarios)
                .Include(t => t.Matriculas)
                .Include(t => t.Disciplina)
                .Where(t => t.SemestreId == semestreId);

            var disciplinasCursadasEstudante = _matriculaContext.MatriculaTurmas
                .Where(mt =>
                    mt.EstudanteId == estudanteId &&
                    mt.Estado == (EstadoMatriculaTurmaEnum.APROVADO ^ EstadoMatriculaTurmaEnum.CURSANDO)
                    )
                .Select(mt => mt.Turma.Disciplina).Distinct();

            return disciplinasComTurmas.Where(d => !disciplinasCursadasEstudante.Contains(d)).ToListAsync();
        }


        public Task<Disciplina> GetDisciplinaByIdWithMatriculasAndSemestre(long id)
        {
            return _matriculaContext.Disciplinas
                .Include(d => d.Turmas).ThenInclude(t => t.Semestre)
                .Include(d => d.Turmas).ThenInclude(t => t.Horarios)
                .Include(d => d.Turmas).ThenInclude(t => t.Matriculas)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
