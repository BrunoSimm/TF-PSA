using Entidades.Modelos;
using System;
using System.Collections.Generic;

namespace MatriculaPUCRS.Models
{
    public class TurmaViewModel : Turma
    {
        public long Id { get; set; }
        public int NumeroDeVagas { get; set; }
        public long DisciplinaId { get; set; }
        public Disciplina Disciplina { get; set; }
        public long SemestreId { get; set; }
        public Semestre Semestre { get; set; }
        public IEnumerable<HorarioGrade> Horarios { get; set; }
        public IEnumerable<MatriculaTurma> Matriculas { get; set; }

        public int VagasRemanescentes
        {
            get
            {
                return NumeroDeVagas;
            }
            set
            {
                VagasRemanescentes = value;
            }
        }
       
    }
}
