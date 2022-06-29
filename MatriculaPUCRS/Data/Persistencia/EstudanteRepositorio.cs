using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System.Collections.Generic;
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

        public Task<Estudante> GetEstudanteByIdAsync(long? id)
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

        public async Task<Estudante> GetEstudanteWithGradeDeHorarioBySemestreIdAsync(long estudanteId, long semestreId)
        {
            Estudante estudante = await _matriculaContext.Estudantes
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Disciplina)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Horarios)
                    .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == estudanteId);
            
            estudante.Matriculas = estudante.Matriculas.Where(mt => mt.Turma.SemestreId == semestreId);
            return estudante;
        }

        public Task<Estudante> GetEstudanteWithHistorico(long? id)
        {
            return _matriculaContext.Estudantes
                .Include(e => e.Curriculo)
                    .ThenInclude(c => c.Disciplinas)
                    .ThenInclude(t => t.Requisitos)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(t => t.Disciplina)
                    .ThenInclude(t => t.Requisitos)
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

            // Verifica se existe matrícula na disciplina e retorna o Estado
            MatriculaTurma matricula = estudante.Matriculas
                .OrderBy(mt => mt.Estado)
                .FirstOrDefault(mt => mt.Turma.DisciplinaId == disciplinaId);
            return matricula?.Estado ?? EstadoMatriculaTurmaEnum.PENDENTE;
        }

        public long GetQuantidadeDeEstudantesAtivosByCurriculoId(long curriculoId)
        {
            return _context.Estudantes.Include(e => e.Curriculo)
                .Where(e => e.Curriculo.Id == curriculoId && e.Estado == EstadoEstudanteEnum.ATIVO)
                .Count();
        }

        public IEnumerable<Estudante> GetEstudantesWithDisciplinasAndCurriculoByCurriculoId(long id)
        {
            return _context.Estudantes
                .Include(e => e.Curriculo)
                .Include(e => e.Matriculas)
                    .ThenInclude(m => m.Turma)
                    .ThenInclude(d => d.Disciplina)
                .Where(e => e.Curriculo.Id == id)
                .AsNoTracking()
                .AsEnumerable();
        }
    }
}
