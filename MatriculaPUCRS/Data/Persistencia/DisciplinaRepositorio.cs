using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class DisciplinaRepositorio : CrudEF<Disciplina, MatriculaContext>, IDisciplinaRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public DisciplinaRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }
        Task<Disciplina> IDisciplinaRepositorio.GetDisciplinaByIdWithMatriculasAndSemestre(long? id)
        {
            return _matriculaContext.Disciplinas
                 .Include(d => d.Turmas).ThenInclude(t => t.Matriculas)
                 .Include(m => m.Turmas).ThenInclude(t => t.Semestre)
                 .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
