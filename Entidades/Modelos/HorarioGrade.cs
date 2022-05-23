using System.Collections.Generic;

namespace Entidades.Modelos
{
    public class HorarioGrade
    {
        public long Id { get; set; }
        public string Horario { get; set; } //6NP 5LM ...
        public virtual IEnumerable<Turma> Turmas { get; set; }
    }
}
