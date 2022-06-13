using System;
using System.Collections.Generic;

namespace Entidades.Modelos
{
    public class HorarioGrade
    {
        public long Id { get; set; }
        public string Horario { get; set; } //6NP 5LM ...
        public virtual IEnumerable<Turma> Turmas { get; set; }
        public string HorarioInicial { get; set; }
        public string HorarioFinal { get; set; }
        public int DiaDaSemana => Convert.ToInt32(Horario[0]);
        public string Periodo => Horario.Substring(Horario.Length - 2);
    }
}
