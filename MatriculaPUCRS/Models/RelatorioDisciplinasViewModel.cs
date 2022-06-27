using Entidades.Modelos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MatriculaPUCRS.Models
{
    public class RelatorioDisciplinasViewModel
    {
        [Display(Name = "Nome da Disciplina")]
        private string NomeDaDisciplina { get; set; }
        private List<Estudante> estudantes { get; set; }
    }
}
