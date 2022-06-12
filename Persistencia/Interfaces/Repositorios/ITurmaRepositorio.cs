using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface ITurmaRepositorio : ICrud<Turma>
    {
        IQueryable<Turma> ListTurmasWithDisciplinaAndSemestreAndHorariosAsQueryable();
        Task<Turma> GetTurmaByIdAsync(long id);
    }
}
