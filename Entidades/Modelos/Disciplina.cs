using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos
{
    public class Disciplina
    {
        public long Id { get; set; }

        [Display(Name = "Código")]
        [RegularExpression(@"^\w{5}-\d{2}$", ErrorMessage = "Código de disciplina inválido.")]
        public string Codigo { get; set; }
        public string Nome { get; set; }
        
        [Display(Name = "Nível")]
        [Range(1, 10, ErrorMessage = "O valor deve estar entre 1 e 10.")]
        public int Nivel { get; set; }

        [Display(Name = "Carga Horária")]
        [Range(0, 3000, ErrorMessage = "O valor deve ser positivo.")]
        public int CargaHoraria { get; set; }
        public IEnumerable<Curriculo> Curriculos { get; set; }
        public IEnumerable<Requisito> Requisitos { get; set; }
        public IEnumerable<Turma> Turmas { get; set; }
        public string NomeParaLista => string.Concat(Codigo, " - ", Nome);

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
