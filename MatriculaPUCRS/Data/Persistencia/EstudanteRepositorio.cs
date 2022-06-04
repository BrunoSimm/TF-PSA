using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System.Linq;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class EstudanteRepositorio : CrudEF<Estudante, MatriculaContext>, IEstudanteRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public EstudanteRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }

        public IQueryable<Estudante> GetById(long id)
        {
            return _matriculaContext.Estudantes.Where(x => x.Id == id);
        }
    }
}
