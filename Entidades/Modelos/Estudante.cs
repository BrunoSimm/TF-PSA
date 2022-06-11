using System.Collections.Generic;

namespace Entidades.Modelos
{
    public class Estudante
    {
        public long Id { get; set; } //Matricula
        public int DigitoVerificador { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public EstadoEstudanteEnum Estado { get; set; }
        public IEnumerable<MatriculaTurma> Matriculas { get; set; }
    }
}
