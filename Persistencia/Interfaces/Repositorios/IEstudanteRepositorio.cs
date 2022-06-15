using Entidades.Modelos;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface IEstudanteRepositorio : ICrud<Estudante>
    {
        Task<Estudante> GetByIdAsync(long? id);
        Task<Estudante> GetByCPF(string cpf);
    }
}
