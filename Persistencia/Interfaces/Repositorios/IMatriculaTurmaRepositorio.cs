using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface IMatriculaTurmaRepositorio : ICrud<MatriculaTurma>
    {
        Task MatricularEstudanteAsync(Turma turma, Estudante estudante);
        Task<MatriculaTurma> GetByEstudanteAndTurma(Estudante estudante, Turma turma);
        Task<List<MatriculaTurma>> GetByEstudanteAndSemestre(Estudante estudante, Semestre semestre);
    }
}
