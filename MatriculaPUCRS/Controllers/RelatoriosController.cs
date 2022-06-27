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
        private readonly MatriculaContext _context;
        private readonly IDisciplinaRepositorio disciplinaRepositorio;
        private readonly ISemestreRepositorio semestreRepositorio;

        public RelatoriosController(MatriculaContext context, IDisciplinaRepositorio disciplinaRepositorio, ISemestreRepositorio semestreRepositorio)
        {
            _context = context;
            this.disciplinaRepositorio = disciplinaRepositorio;
            this.semestreRepositorio = semestreRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Curriculos.ToListAsync());
        }

        public IActionResult Cursos(long? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            RelatorioCursosViewModel relatorio = new RelatorioCursosViewModel();

            var cursos = _context.Curriculos.Where(c => c.Ativo).AsEnumerable();
            ViewBag.Cursos = new SelectList(cursos, nameof(Curriculo.Id), nameof(Curriculo.NomeDoCurso), (long)id);

            var nomeDoCurso = _context.Curriculos.Find((long)id);
            if (nomeDoCurso is null)
            {
                return NotFound();
            }

            relatorio.NomeDoCurso = _context.Curriculos.Find((long)id).NomeParaLista;

            relatorio.QuantidadeDeAlunos = _context.Estudantes.Include(e => e.Curriculo)
                .Where(e => e.Curriculo.Id == (long)id && e.Estado == EstadoEstudanteEnum.ATIVO)
                .Distinct()
                .Count();

            var estudantes = _context.Estudantes.Include(e => e.Curriculo)
                .Include(e => e.Matriculas)
                .ThenInclude(m => m.Turma)
                .ThenInclude(d => d.Disciplina)
                .Where(e => e.Curriculo.Id == (long)id);

            int qtdAlunosMatriculados = 0;
            int totalDeCreditosMatriculados = 0;

            foreach (var estudante in estudantes)
            {
                var matriculas = estudante.Matriculas.Where(m => m.Estado == EstadoMatriculaTurmaEnum.MATRICULADO);

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
            var semestreAtual = await semestreRepositorio.GetSemestreAtualAsync();
            var disciplinasQuery = disciplinaRepositorio.GetDisciplinasIQueryable()
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Semestre)
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Horarios)
                .Include(d => d.Turmas)
                    .ThenInclude(t => t.Matriculas)
                    .ThenInclude(mt => mt.Estudante)
                .Include(d => d.Curriculos)
                .Where(d => d.Curriculos.Any(c => c.Id == 1L))
                .Where(d => d.Turmas.Any(t => t.SemestreId == semestreAtual.Id));

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
            disciplinasList.ForEach(d =>
            {
                d.Turmas = d.Turmas.Where(t => t.SemestreId == semestreAtual.Id);
            });

            return View(disciplinasList);
        }

        public async Task<IActionResult> DisciplinaRelatorio(long id, string? nomeAluno, string? matriculaAluno,
            string? estado, string? curriculo, string? semestreInput)
        {
            Disciplina disciplina = await disciplinaRepositorio.GetDisciplinaByIdWithMatriculasAndSemestre(id);
            ViewBag.Curriculos = new SelectList(disciplina.Curriculos.Select(c => c.Codigo));
            ViewBag.Estados = new SelectList(Enum.GetValues(typeof(EstadoEstudanteEnum)));

            IEnumerable<Semestre> semestres = await semestreRepositorio.List();
            ViewBag.Semestres = new SelectList(semestres.Select(s => s.Titulo));

            if (disciplina is null)
            {
                return NotFound();
            }

            Semestre semestreAtual = await semestreRepositorio.GetSemestreAtualAsync();
            ViewBag.SemestreAtual = semestreAtual;

            Semestre semestre;
            if (semestreInput is not null)
            {
                semestre = semestres.Where(s => s.Titulo.Equals(semestreInput)).FirstOrDefault();
                if (semestre is null)
                {
                    return NotFound();
                }
            }
            else
            {
                semestre = semestreAtual;
            }
            ViewBag.SemestreSelecionado = semestre;
            disciplina.Turmas = disciplina.Turmas.Where(t => t.SemestreId == semestre.Id);

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
                matriculas = matriculas.Where(mt => mt.Estudante.Estado == (EstadoEstudanteEnum)Enum.Parse(typeof(EstadoEstudanteEnum), estado)).ToList();
            }

            if (curriculo is not null && !curriculo.Equals("todos"))
            {
                matriculas = matriculas.Where(mt => mt.Estudante.Curriculo.Codigo == curriculo).ToList();
            }

            IEnumerable<IGrouping<Turma, MatriculaTurma>> matriculasAgrupadas = matriculas.GroupBy(mt => mt.Turma);
            ViewBag.MatriculasAgrupadas = matriculasAgrupadas.ToList();

            return View(disciplina);
        }
    }
}
