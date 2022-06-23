using Entidades.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface ITurmaRepositorio : ICrud<Turma>
    {
        public IEnumerable<Turma> ListTurmasWithDisciplinaAndSemestreAndHorariosAndMatriculas();
        public Task<Turma> GetTurmaByIdAsync(long id);
        public Task<Turma> GetTurmaByIdWithEstudantesAsync(long id);
        public IQueryable<HorarioGrade> GetHorarios();
    }
}
