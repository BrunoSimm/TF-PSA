using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos
{
    public class Estudante
    {
        [Display(Name = "Matrícula")]
        public long Id { get; set; } // Id Matricula
        public int DigitoVerificador { get; }

        [Display(Name = "Matrícula")]
        public string NumeroMatricula => Id + "-" + DigitoVerificador; // Matricula = "Id-DV"
        public string Nome { get; set; }
        public string CPF { get; set; }
        public EstadoEstudanteEnum Estado { get; set; } = EstadoEstudanteEnum.ATIVO;
        public long? CurriculoId { get; set; }
        public Curriculo Curriculo { get; set; }
        public IEnumerable<MatriculaTurma> Matriculas { get; set; }
        public string NomeParaLista => string.Concat(NumeroMatricula, " - ", Nome);
    }
}
