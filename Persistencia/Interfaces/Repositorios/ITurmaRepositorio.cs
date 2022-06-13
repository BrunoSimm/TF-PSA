using Entidades.Modelos;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface ITurmaRepositorio : ICrud<Turma>
    {
        public IQueryable<Turma> ListTurmasWithDisciplinaAndSemestreAndHorariosAsQueryable();
        public Task<Turma> GetTurmaByIdAsync(long id);
    }
}
