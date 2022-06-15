using System.Collections.Generic;

namespace Entidades.Modelos
{
    public class Estudante
    {
        public long Id { get; set; } // Id Matricula
        public int DigitoVerificador { get; }
        public string NumeroMatricula => Id + "-" + DigitoVerificador; // Matricula = "Id-DV"
        public string Nome { get; set; }
        public string CPF { get; set; }
        public EstadoEstudanteEnum Estado { get; set; } = EstadoEstudanteEnum.ATIVO;
        public long? CurriculoId { get; set; }
        public Curriculo Curriculo { get; set; }
        public IEnumerable<MatriculaTurma> Matriculas { get; set; }
    }
}
