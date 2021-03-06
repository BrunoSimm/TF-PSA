using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class MatriculaTurmaRepositorio : CrudEF<MatriculaTurma, MatriculaContext>, IMatriculaTurmaRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public MatriculaTurmaRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }

        public Task<List<MatriculaTurma>> GetByEstudanteAndSemestre(Estudante estudante, Semestre semestre)
        {
            return _matriculaContext.MatriculaTurmas
                .Include(mt => mt.Turma).ThenInclude(t => t.Horarios)
                .Include(mt => mt.Turma).ThenInclude(t => t.Disciplina).ThenInclude(d => d.Curriculos)
                .Include(mt => mt.Estudante).ThenInclude(e => e.Curriculo)
                .Where(mt =>
                    mt.EstudanteId == estudante.Id &&
                    mt.Turma.SemestreId == semestre.Id
                )
                .ToListAsync();
        }

        public async Task<MatriculaTurma> GetByEstudanteAndTurma(long turmaId, long estudanteId)
        {
            return await _matriculaContext.MatriculaTurmas
                .Include(mt => mt.Turma)
                .Include(mt => mt.Estudante)
                .FirstOrDefaultAsync(mt => mt.TurmaId == turmaId && mt.EstudanteId == estudanteId);
        }

        public async Task MatricularEstudanteAsync(Turma turma, Estudante estudante)
        {
            _matriculaContext.MatriculaTurmas.Add(new MatriculaTurma() { 
                Estudante = estudante, EstudanteId = estudante.Id, 
                Turma = turma, TurmaId = turma.Id,
                //Aprovado = false,
                //Nota = 0,
                //Estado = EstadoMatriculaTurmaEnum.MATRICULADO
            });
            await _matriculaContext.SaveChangesAsync();
        }

        public async Task<bool> MatriculaTurmaExists(long turmaId, long estudanteId)
        {
            return await _context.MatriculaTurmas
                .AnyAsync(e => e.TurmaId == turmaId && e.EstudanteId == estudanteId);
        }
    }
}
    
