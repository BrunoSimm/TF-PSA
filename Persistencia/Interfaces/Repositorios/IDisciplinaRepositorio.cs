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
        Task<Disciplina> GetDisciplinaByIdWithMatriculasAndSemestre(long? id);
        Task<List<Disciplina>> GetDisciplinasFromCurrentSemester();
    }
}
