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
        [Display(Name = "Código")]
        public long Id { get; set; }
        
        [Display(Name ="Vagas Ofertadas")]
        public int NumeroDeVagas { get; set; }
        public long DisciplinaId { get; set; }
        public Disciplina Disciplina { get; set; }
        public long SemestreId { get; set; }
        public Semestre Semestre { get; set; }
        [Display(Name ="Horários")]
        public IEnumerable<HorarioGrade> Horarios { get; set; }
        public IEnumerable<MatriculaTurma> Matriculas { get; set; }

        [Display(Name = "Vagas Disponíveis")]
        public int VagasRemanescentes
        {
            get
            {
                if (Matriculas != null)
                {
                    return NumeroDeVagas - Matriculas.Count();
                } else
                {
                    return NumeroDeVagas;
                }
            }
        }
    }
}
