using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos
{
    public class Disciplina
    {
        public long Id { get; set; }

        [Display(Name = "Código")]
        public string Codigo { get; set; }
        public string Nome { get; set; }
        
        [Display(Name = "Nível")]
        public int Nivel { get; set; }

        [Display(Name = "Carga Horária")]
        public int CargaHoraria { get; set; }
        public IEnumerable<Curriculo> Curriculos { get; set; }
        public IEnumerable<Requisito> Requisitos { get; set; }
        public IEnumerable<Turma> Turmas { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Disciplina disciplina&&
                   Codigo==disciplina.Codigo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Codigo);
        }
    }
}
