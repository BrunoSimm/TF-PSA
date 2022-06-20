using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Entidades.Modelos
{
    public class Semestre
    {
        public long Id { get; set; }
        public string Titulo { get; set; } = string.Concat(DateTime.Today.Year, "/", DateTime.Today.Month <= 6 ? "1" : "2" );

        [DataType(DataType.Date)]
        public DateTime DataInicial { get; set; } = DateTime.Today.Month <= 6 ?
            new DateTime(DateTime.Today.Year, 01, 01) :
            new DateTime(DateTime.Today.Year, 01, 07);

        [DataType(DataType.Date)]
        public DateTime DataFinal { get; set; } = DateTime.Today.Month <= 6 ?
            new DateTime(DateTime.Today.Year, 06, 30) :
            new DateTime(DateTime.Today.Year, 12, 31);
        public IEnumerable<Turma> Turmas { get; set; } = new List<Turma>();

        [Display(Name = "Total de Alunos")]
        public int TotalDeAlunos => Turmas.SelectMany(t => t.Matriculas).Select(m => m.Estudante).Distinct().Count();
    }
}
