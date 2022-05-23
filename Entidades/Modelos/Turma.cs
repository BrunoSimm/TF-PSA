using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public class Turma
    {
        public long Id { get; set; }
        public int NumeroDeVagas { get; set; }
        public long DisciplinaId { get; set; }
        public Disciplina Disciplina { get; set; }
        public long SemestreId { get; set; }
        public Semestre Semestre { get; set; }
        public IEnumerable<HorarioGrade> Horarios { get; set; }
        public IEnumerable<MatriculaTurma> Matriculas { get; set; }
    }
}
