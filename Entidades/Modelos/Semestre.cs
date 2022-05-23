using System;
using System.Collections.Generic;

namespace Entidades.Modelos
{
    public class Semestre
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public IEnumerable<Turma> Turmas { get; set; }
    }
}
