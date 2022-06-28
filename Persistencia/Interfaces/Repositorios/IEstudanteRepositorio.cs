using Entidades.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface IEstudanteRepositorio : ICrud<Estudante>
    {
        public Task<Estudante> GetByIdAsync(long? id);
        public Task<Estudante> GetEstudanteWithHistorico(long? id);
        public Task<Estudante> GetByCPF(string cpf);
        public Task<EstadoMatriculaTurmaEnum?> GetStatusDisciplina(long estudanteId, long disciplinaId);
        public long GetQuantidadeDeEstudantesAtivosByCurriculoId(long curriculoId);
        public IEnumerable<Estudante> GetEstudantesWithDisciplinasAndCurriculoByCurriculoId(long id);

    }
}
