using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
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

        public Task<Turma> GetTurmaByIdAsync(long id)
        {
            return _matriculaContext.Turmas.Include(t => t.Horarios).Include(t => t.Disciplina).ThenInclude(d => d.Requisitos).ThenInclude(r => r.Disciplina).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<Turma> ListTurmasWithDisciplinaAndSemestreAndHorariosAsQueryable()
        {
            return _matriculaContext.Turmas.Include(t => t.Disciplina).ThenInclude(d => d.Requisitos).Include(t => t.Semestre).Include(t => t.Horarios).AsQueryable();
        }

    }
}
