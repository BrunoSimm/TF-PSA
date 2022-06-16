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
                .ToListAsync();
        }

        public Task<Disciplina> GetDisciplinaByIdWithMatriculasAndSemestre(long? id)
        {
            return _matriculaContext.Disciplinas
                .Include(d => d.Turmas).ThenInclude(t => t.Semestre)
                .Include(d => d.Turmas).ThenInclude(t => t.Matriculas)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
