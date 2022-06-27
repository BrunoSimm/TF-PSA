using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class EstudanteRepositorio : CrudEF<Estudante, MatriculaContext>, IEstudanteRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public EstudanteRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }

        public Task<Estudante> GetByCPF(string cpf)
        {
            return _matriculaContext.Estudantes.SingleOrDefaultAsync(e => e.CPF.Equals(cpf));
        }

        public Task<Estudante> GetByIdAsync(long? id)
        {
            return _matriculaContext.Estudantes
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Disciplina)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Horarios)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public Task<Estudante> GetEstudanteWithHistorico(long? id)
        {
            return _matriculaContext.Estudantes
                .Include(e => e.Curriculo)
                    .ThenInclude(c => c.Disciplinas)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Disciplina)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Horarios)
                    .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Semestre)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<EstadoMatriculaTurmaEnum?> GetStatusDisciplina(long estudanteId, long disciplinaId)
        {
            // Verifica se existe a disciplina no currículo
            Estudante estudante = await _matriculaContext.Estudantes
                .Include(e => e.Curriculo)
                    .ThenInclude(c => c.Disciplinas)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Disciplina)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Horarios)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Semestre)
                .SingleOrDefaultAsync(e => e.Id == estudanteId);
            Disciplina disciplinaCurriculo = estudante.Curriculo.Disciplinas.FirstOrDefault(d => d.Id == disciplinaId);
            if (disciplinaCurriculo is null) return null;

            // Verifica se existe matricula na disciplina e retorna o Estado
            IOrderedEnumerable<MatriculaTurma> matriculas = estudante.Matriculas.OrderByDescending(mt => mt.Turma.Semestre.DataFinal);
            MatriculaTurma matricula = matriculas.FirstOrDefault(mt => mt.Turma.DisciplinaId == disciplinaId);
            return matricula is null ? EstadoMatriculaTurmaEnum.PENDENTE : matricula.Estado;
        }
    }
}
