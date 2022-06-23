using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class TurmaRepositorio : CrudEF<Turma, MatriculaContext>, ITurmaRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public TurmaRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }

        public IQueryable<HorarioGrade> GetHorarios()
        {
            return _matriculaContext.HorariosGrade.OrderBy(hg => hg.Horario).AsQueryable();
        }

        public Task<Turma> GetTurmaByIdAsync(long id)
        {
            return _matriculaContext.Turmas
                .Include(t => t.Horarios)
                .Include(t => t.Disciplina)
                    .ThenInclude(d => d.Requisitos)
                        .ThenInclude(r => r.Disciplina)
                .Include(t => t.Semestre)
                .Include(t => t.Matriculas)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public Task<Turma> GetTurmaByIdWithEstudantesAsync(long id)
        {
            return _matriculaContext.Turmas
                .Include(t => t.Horarios)
                .Include(t => t.Disciplina)
                    .ThenInclude(d => d.Requisitos)
                        .ThenInclude(r => r.Disciplina)
                .Include(t => t.Semestre)
                .Include(t => t.Matriculas)
                    .ThenInclude(mt => mt.Estudante)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public IEnumerable<Turma> ListTurmasWithDisciplinaAndSemestreAndHorariosAndMatriculas()
        {
            return _matriculaContext.Turmas
                .Include(t => t.Disciplina)
                    .ThenInclude(d => d.Requisitos)
                .Include(t => t.Semestre)
                .Include(t => t.Horarios)
                .Include(t => t.Matriculas)
                .AsEnumerable();
        }

        public override async Task Update(Turma turma)
        {
            var horarios = turma.Horarios;
            turma.Horarios = null;
            await base.Update(turma);
            _matriculaContext.Entry(turma).Collection(t => t.Horarios).Load();
            turma.Horarios = horarios;
            await _context.SaveChangesAsync();
        }
    }
}
