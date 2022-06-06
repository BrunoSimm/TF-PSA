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
        private readonly MatriculaContext _context;
        public MatriculaTurmaRepositorio(MatriculaContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<MatriculaTurma>> GetByEstudanteAndSemestre(Estudante estudante, Semestre semestre)
        {
            return _context.MatriculaTurmas
                .Include(mt => mt.Turma).ThenInclude(t => t.Horarios)
                .Where(
                    mt => mt.EstudanteId == estudante.Id &&
                    mt.Turma.SemestreId == semestre.Id
                )
                .ToListAsync();    
        }

        public async Task<MatriculaTurma> GetByEstudanteAndTurma(Estudante estudante, Turma turma)
        {
            return await _context.MatriculaTurmas.Where(mt => mt.EstudanteId == estudante.Id && mt.Turma.Id == turma.Id).FirstOrDefaultAsync();
        }

        public async Task MatricularEstudanteAsync(Turma turma, Estudante estudante)
        {
            _context.MatriculaTurmas.Add(new MatriculaTurma() { 
                Estudante = estudante, EstudanteId = estudante.Id, 
                Turma = turma, TurmaId = turma.Id,
                Aprovado = false,
                Nota = 0
            });
            await _context.SaveChangesAsync();
        }
    }
}
    
