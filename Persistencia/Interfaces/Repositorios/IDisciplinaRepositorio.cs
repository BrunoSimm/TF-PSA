using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface IDisciplinaRepositorio : ICrud<Disciplina>
    {
        public Task<Disciplina> GetDisciplinaByIdWithMatriculasAndSemestre(long id);
        public IEnumerable<IGrouping<Disciplina, Turma>> GetDisciplinasWithTurmasFromSemestre(long estudanteId, long semestreId);
        public Task<Curriculo> GetDisciplinasFromCurriculoId(long id);
        public IQueryable<Disciplina> GetDisciplinasIQueryable();
    }
}
