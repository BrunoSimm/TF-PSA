using System.ComponentModel.DataAnnotations;

namespace MatriculaPUCRS.Models
{
    public class AlunoViewModel
    {
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Display(Name = "Nome do Aluno")]
        public string NomeDoAluno { get; set; }

        [Display(Name = "Créditos com Aprovação")]
        public int CreditosComAprovacao { get; set; }

        [Display(Name = "Coeficiente de Rendimento")]
        public double CoeficienteDeRendimento { get; set; }
    }
}
