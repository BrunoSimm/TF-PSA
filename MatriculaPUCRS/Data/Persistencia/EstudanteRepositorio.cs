using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class EstudanteRepositorio : CrudEF<Estudante, MatriculaContext>, IEstudanteRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public EstudanteRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }

        public async Task<Estudante> GetByIdAsync(long id)
        {
            return await _matriculaContext.Estudantes
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Disciplina)
                .SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}
