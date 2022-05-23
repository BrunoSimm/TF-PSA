using Entidades.Modelos;
using Infraestrutura.Data;
using Persistencia.Interfaces.Repositorios;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class DisciplinaRepositorio: CrudEF<Disciplina, MatriculaContext>, IDisciplinaRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public DisciplinaRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }
    }
}
