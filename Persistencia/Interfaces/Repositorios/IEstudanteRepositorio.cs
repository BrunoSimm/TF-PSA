using Entidades.Modelos;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface IEstudanteRepositorio : ICrud<Estudante>
    {
        public Task<Estudante> GetByIdAsync(long? id);
        public Task<Estudante> GetByCPF(string cpf);
        public Task<EstadoMatriculaTurmaEnum?> GetStatusDisciplina(long estudanteId, long disciplinaId);
    }
}
