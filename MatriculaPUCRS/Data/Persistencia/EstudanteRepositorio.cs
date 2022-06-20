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

        public Task<Estudante> GetByCPF(string cpf)
        {
            return _matriculaContext.Estudantes.SingleOrDefaultAsync(e => e.CPF.Equals(cpf));
        }

        public Task<Estudante> GetByIdAsync(long? id)
        {
            return _matriculaContext.Estudantes
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Disciplina)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Horarios)
                .SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}
