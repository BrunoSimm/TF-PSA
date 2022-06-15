using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos
{
    public class Semestre
    {
        public long Id { get; set; }
        public string Titulo { get; set; } = string.Concat(DateTime.Today.Year, "/", DateTime.Today.Month <= 6 ? "1" : "2" );

        [DataType(DataType.Date)]
        public DateTime DataInicial { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime DataFinal { get; set; } = DateTime.Today.Month <= 6 ?
            new DateTime(DateTime.Today.Year, 06, 30) :
            new DateTime(DateTime.Today.Year, 12, 31);
        public IEnumerable<Turma> Turmas { get; set; }
    }
}
