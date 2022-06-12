using System.Collections.Generic;

namespace Entidades.Modelos
{
    public class Estudante
    {
        public long Id { get; set; } // Id Matricula
        public int DigitoVerificador { get; }
        public string NumeroMatricula // Matricula = "Id-DV"
        {
            get => Id + "-" + DigitoVerificador;
        }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public EstadoEstudanteEnum Estado { get; set; }
        public IEnumerable<MatriculaTurma> Matriculas { get; set; }
    }
}
