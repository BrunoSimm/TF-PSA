using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System.Linq;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class SemestreRepositorio : CrudEF<Semestre, MatriculaContext>, ISemestreRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public SemestreRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }
    }
}
