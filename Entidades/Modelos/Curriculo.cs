using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos
{
    public class Curriculo
    {
        public long Id { get; set; }

        [Display(Name = "Nome do Curso")]
        public string NomeDoCurso { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
        public IEnumerable<Estudante> Estudantes { get; set; }
        public IEnumerable<Disciplina> Disciplinas { get; set; }

        [Display(Name = "Currículo")]
        public string NomeParaLista => string.Concat(Codigo, " - ", NomeDoCurso);
    }
}
