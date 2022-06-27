using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MatriculaPUCRS.Models
{
    public class RelatorioAlunosViewModel
    {
        public string NomeDoCurso { get; set; }
        public List<AlunoViewModel> Alunos { get; set; }
    }
}
