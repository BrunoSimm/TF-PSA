using Entidades.Modelos;
using Infraestrutura.Data;
using Persistencia.Interfaces.Repositorios;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class TurmaRepositorio : CrudEF<Turma, MatriculaContext>, ITurmaRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public TurmaRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }
    }
}
