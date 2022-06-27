using System.ComponentModel.DataAnnotations;

namespace MatriculaPUCRS.Models
{
    public class AlunoViewModel
    {
        [Display(Name = "Nome do Aluno")]
        public string NomeDoAluno { get; set; }
        [Display(Name = "Número de Créditos com Aprovação")]
        public int NumeroDeCreditosComAprovacao { get; set; }
        [Display(Name = "Coeficiente de Rendimento")]
        public double CoeficienteDeRendimento { get; set; }
    }
}
