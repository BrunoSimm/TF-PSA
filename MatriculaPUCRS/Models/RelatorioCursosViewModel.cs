using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Models
{
    public class RelatorioCursosViewModel
    {
        public string NomeDoCurso { get; set; }
        [Display(Name = "Total de Alunos")]
        public long QuantidadeDeAlunos { get; set; }
        [Display(Name = "Percentual de Alunos Matriculados")]
        public double PercentualDeMatriculas { get; set; }
        [Display(Name = "Média de Créditos")]
        public double MediaDeCreditosPorAluno { get; set; }
    }
}
