using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface ISemestreRepositorio : ICrud<Semestre>
    {
        public Task<Semestre> GetSemestreAtualAsync();
        public Task<IEnumerable<Semestre>> ListSemestresWithTurmas();
        public Task<Semestre> GetSemestreWithTurmasById(long Id);
    }


}
