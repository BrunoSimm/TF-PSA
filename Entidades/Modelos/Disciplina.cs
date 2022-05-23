using System;
using System.Collections.Generic;

namespace Entidades.Modelos
{
    public class Disciplina
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public IEnumerable<Curriculo> Curriculos { get; set; }
        public IEnumerable<Requisito> Requisitos { get; set; }
        public IEnumerable<Turma> Turmas { get; set; }
    }
}
