using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class SemestreRepositorio : CrudEF<Semestre, MatriculaContext>, ISemestreRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public SemestreRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }

        public async Task<Semestre> GetSemestreAtualAsync()
        {
            return await _matriculaContext.Semestres.Where(s => DateTime.Now >= s.DataInicial && DateTime.Now <= s.DataFinal).FirstOrDefaultAsync();
        }
    }
}
