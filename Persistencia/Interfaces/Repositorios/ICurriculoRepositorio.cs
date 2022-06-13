using Entidades.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface ICurriculoRepositorio : ICrud<Curriculo>
    {
        public Task<List<Curriculo>> GetActiveCurriculosAsync();
    }
}
