using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos;
using Infraestrutura.Data;
using MatriculaPUCRS.Models;
using Microsoft.AspNetCore.Authorization;
using Persistencia.Interfaces.Repositorios;

namespace MatriculaPUCRS.Controllers
{
    [Authorize(Roles = "Coordenador")]
    public class RelatoriosController : Controller
    {
        private readonly ICurriculoRepositorio _curriculoRepositorio;
        private readonly IDisciplinaRepositorio _disciplinaRepositorio;
        private readonly ISemestreRepositorio _semestreRepositorio;
        private readonly IEstudanteRepositorio _estudanteRepositorio;

        public RelatoriosController(ICurriculoRepositorio curriculoRepositorio, IDisciplinaRepositorio disciplinaRepositorio,
            ISemestreRepositorio semestreRepositorio, IEstudanteRepositorio estudanteRepositorio)
        {
            this._curriculoRepositorio = curriculoRepositorio;
            this._disciplinaRepositorio = disciplinaRepositorio;
            this._semestreRepositorio = semestreRepositorio;
            this._estudanteRepositorio = estudanteRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _curriculoRepositorio.GetActiveCurriculosAsync());
        }

        public async Task<IActionResult> RelatorioMatricula(long? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            RelatorioCursosViewModel relatorio = new RelatorioCursosViewModel();

            var cursos = await _curriculoRepositorio.GetActiveCurriculosAsync();
            ViewBag.Cursos = new SelectList(cursos, nameof(Curriculo.Id), nameof(Curriculo.NomeDoCurso), (long)id);

            var curso = await _curriculoRepositorio.GetEntityById((long)id);
            if (curso is null)
            {
                return NotFound();
            }

            Semestre semestre = await _semestreRepositorio.GetSemestreAtualAsync();
            if (semestre is null)
            {
                return NotFound();
            }

            relatorio.NomeDoCurso = curso.NomeParaLista;

            relatorio.QuantidadeDeAlunos = _estudanteRepositorio.GetQuantidadeDeEstudantesAtivosByCurriculoId((long)id);

            var estudantesDoCurso = _estudanteRepositorio.GetEstudantesWithDisciplinasAndCurriculoByCurriculoId((long)id);

            int qtdAlunosMatriculados = 0;
            int totalDeCreditosMatriculados = 0;

            foreach (var estudante in estudantesDoCurso)
            {
                var matriculas = estudante.Matriculas.Where(m => m.Turma.SemestreId == semestre.Id);

                if (matriculas.Any())
                {
                    qtdAlunosMatriculados += 1;
                    foreach (var matricula in matriculas)
                    {
                        totalDeCreditosMatriculados += int.Parse(matricula.Turma.Disciplina.Codigo.Substring(6));
                    }
                }
            }

            relatorio.PercentualDeMatriculas = (double)qtdAlunosMatriculados / relatorio.QuantidadeDeAlunos;

            relatorio.MediaDeCreditosPorAluno = (double)totalDeCreditosMatriculados / qtdAlunosMatriculados;

            return View(relatorio);
        }

        public async Task<IActionResult> Disciplinas(string nomeDisciplina, string codigoDisciplina)
        {
            var disciplinasQuery = _disciplinaRepositorio.GetDisciplinasIQueryable()
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Semestre)
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Horarios)
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Matriculas)
                    .ThenInclude(mt => mt.Estudante)
                .Include(d => d.Curriculos)
                .Where(d => d.Curriculos.Any(c => c.Id == 1L));

            if (nomeDisciplina is not null)
            {
                TempData["nomeDisciplinaInput"] = nomeDisciplina;
                disciplinasQuery = disciplinasQuery.Where(d => d.Nome.Contains(nomeDisciplina));
            }

            if (codigoDisciplina is not null)
            {
                TempData["codigoDisciplinaInput"] = codigoDisciplina;
                disciplinasQuery = disciplinasQuery.Where(d => d.Codigo.Contains(codigoDisciplina));
            }

            List<Disciplina> disciplinasList = disciplinasQuery.AsNoTracking().ToList();
            
            ViewBag.Curriculo = await _curriculoRepositorio.GetEntityById(1L);

            return View(disciplinasList);
        }

        public async Task<IActionResult> RelatorioDisciplina(long id, string? nomeAluno, string? matriculaAluno,
            string? estado, long? curriculo, long? semestreId)
        {
            Disciplina disciplina = await _disciplinaRepositorio.GetDisciplinaByIdWithMatriculasAndSemestre(id);
            IEnumerable<Curriculo> curriculos = disciplina.Curriculos;
            ViewBag.Curriculos = new SelectList(curriculos, nameof(Curriculo.Id), nameof(Curriculo.NomeParaLista), curriculo);

            IEnumerable<string> estados = Enum.GetValues(typeof(EstadoMatriculaTurmaEnum)).Cast<EstadoMatriculaTurmaEnum>().Select(e => e.ToString());
            ViewBag.Estados = new SelectList(estados, estado);

            IEnumerable<Semestre> semestres = _semestreRepositorio.List().Result.Where(s => DateTime.Today >= s.DataInicial);
            ViewBag.Semestres = new SelectList(semestres, nameof(Semestre.Id), nameof(Semestre.Titulo), semestreId);

            if (disciplina is null)
            {
                return NotFound();
            }

            Semestre semestre = null;
            if (semestreId is not null)
            {
                semestre = semestres.FirstOrDefault(s => s.Id == semestreId);
            }

            if (semestre is not null)
            {
                disciplina.Turmas = disciplina.Turmas.Where(t => t.SemestreId == semestre.Id);
            }

            List<MatriculaTurma> matriculas = disciplina.Turmas.SelectMany(t => t.Matriculas).ToList();

            //estudantes = estudantes.Where(e => e.Matriculas.Any(m => m.Turma.SemestreId == semestreId)).ToList();

            if (nomeAluno is not null)
            {
                TempData["nomeAlunoInput"] = nomeAluno;
                matriculas = matriculas.Where(mt => mt.Estudante.Nome.Contains(nomeAluno, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (matriculaAluno is not null)
            {
                TempData["matriculaAlunoInput"] = matriculaAluno;
                matriculas = matriculas.Where(mt => mt.Estudante.NumeroMatricula.Contains(matriculaAluno, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (estado is not null && !estado.Equals("todos"))
            {
                matriculas = matriculas.Where(mt => mt.Estado.ToString().Equals(estado)).ToList();
            }

            if (curriculo is not null && !curriculo.Equals("todos"))
            {
                matriculas = matriculas.Where(mt => mt.Estudante.Curriculo.Id == curriculo).ToList();
            }

            IEnumerable<IGrouping<Turma, MatriculaTurma>> matriculasAgrupadas = matriculas.GroupBy(mt => mt.Turma);
            ViewBag.MatriculasAgrupadas = matriculasAgrupadas.ToList();

            return View(disciplina);
        }

        public async Task<IActionResult> Alunos()
        {
            return View(await _curriculoRepositorio.GetActiveCurriculosAsync());
        }

        public async Task<IActionResult> RelatorioAlunos(long? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var curso = await _curriculoRepositorio.GetEntityById((long)id);

            RelatorioAlunosViewModel relatorio = new RelatorioAlunosViewModel()
            {
                Alunos = new List<AlunoViewModel>(),
                NomeDoCurso = curso.NomeParaLista
            };

            var estudantesDoCurso = _estudanteRepositorio.GetEstudantesWithDisciplinasAndCurriculoByCurriculoId((long)id);

            foreach (var estudante in estudantesDoCurso)
            {
                AlunoViewModel aluno = new AlunoViewModel();
                aluno.Matricula = estudante.NumeroMatricula;
                aluno.NomeDoAluno = estudante.Nome;
                float notas = 0;
                int qtdDisciplinasCursadas = 0;

                foreach (var matricula in estudante.Matriculas)
                {
                    if (matricula.Estado == EstadoMatriculaTurmaEnum.APROVADO
                        || matricula.Estado == EstadoMatriculaTurmaEnum.REPROVADO)
                    {
                        notas += matricula.Nota ?? 0;
                        qtdDisciplinasCursadas++;

                        if (matricula.Estado == EstadoMatriculaTurmaEnum.APROVADO)
                            aluno.CreditosComAprovacao += int.Parse(matricula.Turma.Disciplina.Codigo.Substring(6));
                    }
                }
                if (qtdDisciplinasCursadas > 0)
                {
                    aluno.CoeficienteDeRendimento = (double)(notas / qtdDisciplinasCursadas);
                }
                else
                {
                    aluno.CoeficienteDeRendimento = 0;
                }

                relatorio.Alunos.Add(aluno);
            }

            return View(relatorio);
        }

        public async Task<IActionResult> GradesDeHorario(long? estudanteId)
        {
            Semestre semestre = await _semestreRepositorio.GetSemestreAtualAsync();

            Estudante estudante = null;
            if (semestre is not null && estudanteId is not null)
            {
                estudante = await _estudanteRepositorio.GetEstudanteWithGradeDeHorarioBySemestreIdAsync((long)estudanteId, semestre.Id);
            }

            ViewBag.Semestre = semestre;
            IEnumerable<Estudante> estudantes = _estudanteRepositorio.List().Result.Where(e => e.Estado == EstadoEstudanteEnum.ATIVO);

            ViewBag.Estudantes = new SelectList(estudantes, nameof(Estudante.Id), nameof(Estudante.NomeParaLista), estudanteId);

            return View(estudante);
        }
    }
}
