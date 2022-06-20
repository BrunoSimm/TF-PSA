using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public class Turma
    {
        [Display(Name = "Turma")]
        public long Id { get; set; }
        
        [Display(Name = "Vagas")]
        [Range(0, 120, ErrorMessage = "Número máximo deve ser de 120 vagas.")]
        public int NumeroDeVagas { get; set; }
        public long DisciplinaId { get; set; }
        public Disciplina Disciplina { get; set; }
        public long SemestreId { get; set; }
        public Semestre Semestre { get; set; }

        [Display(Name = "Horários")]
        public IEnumerable<HorarioGrade> Horarios { get; set; } = new List<HorarioGrade>();
        
        [Display(Name = "Estudantes Matriculados")]
        public IEnumerable<MatriculaTurma> Matriculas { get; set; } = new List<MatriculaTurma>();

        [Display(Name = "Vagas Disponíveis")]
        public int VagasRemanescentes => NumeroDeVagas - Matriculas.Count();
        
        [Display(Name = "Matriculados")]
        public int NumeroDeMatriculas => Matriculas.Count();
    }
}
