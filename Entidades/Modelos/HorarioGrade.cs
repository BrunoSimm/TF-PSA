using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos
{
    public class HorarioGrade
    {
        public long Id { get; set; }

        [Display(Name = "Período")]
        public string Horario { get; set; } //6NP 5LM ...
        public virtual IEnumerable<Turma> Turmas { get; set; }
        
        [Display(Name = "Horário Inicial")] 
        public string HorarioInicial { get; set; }
        
        [Display(Name = "Horário Final")] 
        public string HorarioFinal { get; set; }
        public int DiaDaSemana => Convert.ToInt32(Horario[0]);
        public string Periodo => Horario.Substring(Horario.Length - 2);
    }
}
