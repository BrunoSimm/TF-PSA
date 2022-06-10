using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public class Requisito
    {
        public long Id { get; set; }
        public TipoRequisitoEnum TipoRequisito { get; set; }
        public long DisciplinaId { get; set; }
        public Disciplina Disciplina { get; set; }
        //public long DisciplinaRequisitoId { get; set; } //rever
        //public Disciplina DisciplinaRequisito { get; set; } //rever
    }
}
