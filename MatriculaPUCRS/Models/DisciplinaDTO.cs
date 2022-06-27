using Entidades.Modelos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MatriculaPUCRS.Models
{
    public class DisciplinaDTO
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public int CargaHoraria { get; set; }
        public ICollection<Curriculo> Curriculos { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Disciplina> DisciplinaOrigem { get; set; }
        public virtual ICollection<Disciplina> Requisitos { get; set; }
        public ICollection<TurmaDTO> Turmas { get; set; }
    }

    public class TurmaDTO
    {
        public long Id { get; set; }
        public int NumeroDeVagas { get; set; }
        public long SemestreId { get; set; }
        public Semestre Semestre { get; set; }
        public ICollection<HorarioGrade> Horarios { get; set; } = new List<HorarioGrade>();
        [JsonIgnore]
        public ICollection<MatriculaTurma> Matriculas { get; set; } = new List<MatriculaTurma>();
        public int VagasRemanescentes => NumeroDeVagas - Matriculas.Count();
        public int NumeroDeMatriculas => Matriculas.Count();
    }
}
