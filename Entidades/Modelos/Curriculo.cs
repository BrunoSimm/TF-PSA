using System.Collections.Generic;

namespace Entidades.Modelos
{
    public class Curriculo
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
        public IEnumerable<Disciplina> Disciplinas { get; set; }
    }
}
