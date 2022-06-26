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
using Persistencia.Interfaces.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MatriculaPUCRS.Controllers
{
    public class DisciplinasController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDisciplinaRepositorio _disciplinaRepositorio;
        private readonly IMatriculaTurmaRepositorio _matriculaTurmaRepositorio;
        private readonly IEstudanteRepositorio _estudanteRepositorio;

        public DisciplinasController(
            UserManager<ApplicationUser> userManager,
            IDisciplinaRepositorio disciplinaRepositorio,
            IMatriculaTurmaRepositorio matriculaTurmaRepositorio,
            IEstudanteRepositorio estudanteRepositorio)
        {
            _userManager = userManager;
            _disciplinaRepositorio = disciplinaRepositorio;
            _matriculaTurmaRepositorio = matriculaTurmaRepositorio;
            _estudanteRepositorio = estudanteRepositorio;
        }

    // GET: Disciplinas
    public async Task<IActionResult> Index()
        {
            Curriculo curso = await _disciplinaRepositorio.GetDisciplinasFromCurriculoId(1);
            return View(curso);
        }

        // GET: Disciplinas/HistoricoEscolar
        [Authorize(Roles = "Estudante")]
        public async Task<IActionResult> HistoricoEscolar()
        {
            // calcular coef de rendimento
            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            Estudante estudante = await _estudanteRepositorio.GetEstudanteWithHistorico(loggedUser.EstudanteId);

            if (estudante is null)
            {
                return NotFound();
            }

            IEnumerable<MatriculaTurma> matriculasTurmas = estudante.Matriculas.Where(mt => (mt.Estado == EstadoMatriculaTurmaEnum.APROVADO) || (mt.Estado == EstadoMatriculaTurmaEnum.REPROVADO)).OrderBy(mt => mt.Estado);
            IEnumerable<IGrouping<int, Disciplina>> disciplinas = estudante.Curriculo.Disciplinas.GroupBy(d => d.Nivel).OrderBy(n => n.Key);

            float? coeficienteDeRendimento = matriculasTurmas.GroupBy(mt => mt.Turma.Disciplina).Average(gd => gd.First().Nota);
            int chAprovado = estudante.Matriculas.Where(mt => mt.Estado == EstadoMatriculaTurmaEnum.APROVADO).Select(mt => mt.Turma.Disciplina).Distinct().Sum(d => d.CargaHoraria);
            int chTotal = estudante.Curriculo.Disciplinas.Sum(d => d.CargaHoraria);
            float percentualDeConclusao = (float) chAprovado / chTotal;

            ViewBag.Matriculas = matriculasTurmas;
            ViewBag.Disciplinas = disciplinas;
            ViewBag.CoeficienteDeRendimento = coeficienteDeRendimento;
            ViewBag.TotalDeHorasCursadas = chAprovado;
            ViewBag.PercentualDeConclusao = percentualDeConclusao;

            return View(estudante);
        }

        // GET: Disciplinas/HistoricoMatriculas
        [Authorize(Roles = "Estudante")]
        public async Task<IActionResult> HistoricoMatriculas()
        {
            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            Estudante estudante = await _estudanteRepositorio.GetEstudanteWithHistorico(loggedUser.EstudanteId);

            if (estudante is null)
            {
                return NotFound();
            }

            return View(estudante);
        }

        // GET: Disciplinas/Details/5
        [Authorize(Roles = "Coordenador")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Disciplina disciplina = await _disciplinaRepositorio.GetDisciplinaByIdWithMatriculasAndSemestre((long) id);
            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // GET: Disciplinas/Create
        [Authorize(Roles = "Coordenador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disciplinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordenador")]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao")] Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                await _disciplinaRepositorio.Add(disciplina);
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        // GET: Disciplinas/Edit/5
        [Authorize(Roles = "Coordenador")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _disciplinaRepositorio.GetEntityById(id);
            if (disciplina == null)
            {
                return NotFound();
            }
            return View(disciplina);
        }

        // POST: Disciplinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordenador")]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Codigo,Nome,Nivel,CargaHoraria")] Disciplina disciplina)
        {
            if (id != disciplina.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _disciplinaRepositorio.Update(disciplina);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _disciplinaRepositorio.EntityExistsById(disciplina.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        // GET: Disciplinas/Delete/5
        [Authorize(Roles = "Coordenador")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplina = await _disciplinaRepositorio.GetEntityById(id);
            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        // POST: Disciplinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordenador")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var disciplina = await _disciplinaRepositorio.GetEntityById(id);
            await _disciplinaRepositorio.Delete(disciplina);
            return RedirectToAction(nameof(Index));
        }
    }
}
