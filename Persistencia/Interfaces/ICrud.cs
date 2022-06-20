using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.Interfaces
{
    public interface ICrud<T> where T : class
    {
        Task Add(T Objeto);
        Task Update(T Objeto);
        Task Delete(T Objeto);
        Task<T> GetEntityById(params object[] Id);
        Task<bool> EntityExistsById(params object[] Id);
        Task<IEnumerable<T>> List();
    }
}
