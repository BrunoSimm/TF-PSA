using Entidades.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface ITurmaRepositorio : ICrud<Turma>
    {
        public IQueryable<Turma> ListTurmasWithDisciplinaAndSemestreAndHorariosAndMatriculasAsQueryable();
        public Task<Turma> GetTurmaByIdAsync(long id);
        public Task<Turma> GetTurmaByIdWithEstudantesAsync(long id);
        public IQueryable<HorarioGrade> GetHorarios();
    }
}
