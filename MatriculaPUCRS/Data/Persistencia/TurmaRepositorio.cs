using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System.Linq;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class TurmaRepositorio : CrudEF<Turma, MatriculaContext>, ITurmaRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public TurmaRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }
        public IQueryable<Turma> ListTurmasWithDisciplinaAndSemestreAndHorariosAsQueryable()
        {
            return _matriculaContext.Turmas.Include(t => t.Disciplina).Include(t => t.Semestre).Include(t => t.Horarios).AsQueryable();
        }
    }
}
