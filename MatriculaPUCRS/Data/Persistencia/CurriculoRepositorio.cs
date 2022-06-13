using Entidades.Modelos;
using Infraestrutura.Data;
using Persistencia.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class CurriculoRepositorio : CrudEF<Curriculo, MatriculaContext>, ICurriculoRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public CurriculoRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }
        public async Task<List<Curriculo>> GetActiveCurriculosAsync()
        {
            return await _matriculaContext.Curriculos.AsNoTracking().Where(c => c.Ativo).ToListAsync();
        }
    }
}
