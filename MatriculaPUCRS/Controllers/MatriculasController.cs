using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos;
using Infraestrutura.Data;
using Persistencia.Interfaces.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MatriculaPUCRS.Models;

namespace MatriculaPUCRS.Controllers
{
    [Authorize(Roles = "Estudante")]
    public class MatriculasController : Controller
    {
        private readonly MatriculaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDisciplinaRepositorio _disciplinaRepositorio;
        private readonly ITurmaRepositorio _turmaRepositorio;
        private readonly IMatriculaTurmaRepositorio _matriculaTurmaRepositorio;
        private readonly IEstudanteRepositorio _estudanteRepositorio;
        private readonly ISemestreRepositorio _semestreRepositorio;

        public MatriculasController(
            UserManager<ApplicationUser> userManager,
            MatriculaContext context,
            IDisciplinaRepositorio disciplinaRepositorio,
            ITurmaRepositorio turmaRepositorio,
            IMatriculaTurmaRepositorio matriculaTurmaRepositorio,
            IEstudanteRepositorio estudanteRepositorio,
            ISemestreRepositorio semestreRepositorio)
        {
            _semestreRepositorio = semestreRepositorio;
            _userManager = userManager;
            _context = context;
            _disciplinaRepositorio = disciplinaRepositorio;
            _turmaRepositorio = turmaRepositorio;
            _matriculaTurmaRepositorio = matriculaTurmaRepositorio;
            _estudanteRepositorio = estudanteRepositorio;
        }

        // GET: Matriculas
        public async Task<IActionResult> Index()
        {
            ViewBag.MatriculaSuccessMessage = TempData["ErrorMessageTemp"];
            List<Disciplina> disciplinas = await _disciplinaRepositorio.GetDisciplinasFromCurrentSemester();
            disciplinas = disciplinas.Distinct().ToList();
            return View(disciplinas);
        }

        // GET: Matriculas/Details/5
        public async Task<IActionResult> Details(long? id, string sortOrder)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ErrorMessage = TempData["ErrorMessageTemp"];
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "titulo_asc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["HorarioSortParm"] = sortOrder == "Horario" ? "horario_desc" : "Horario";
            var turmas = _turmaRepositorio
                .ListTurmasWithDisciplinaAndSemestreAndHorariosAsQueryable()
                .Include(t => t.Matriculas)
                .Where(t => t.DisciplinaId == id)
                .Where(t => DateTime.Now >= t.Semestre.DataInicial && DateTime.Now <= t.Semestre.DataFinal);

            switch (sortOrder)
            {
                case "titulo_asc":
                    turmas = turmas.OrderBy(t => t.Disciplina.Nome);
                    break;
                case "horario_desc":
                    turmas = turmas.OrderByDescending(t => t.Horarios.OrderBy(h => h.Horario).First().Horario);
                    break;
                case "id_desc":
                    turmas = turmas.OrderByDescending(t => t.Id);
                    break;
                default:
                    turmas = turmas.OrderBy(t => t.Id);
                    break;
            }

            if (turmas.Count() == 0)
            {
                return NotFound();
            }
            return View(await turmas.ToListAsync());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Matricular(long turmaId, long disciplinaId, long semestreId)
        {
            Turma turma = await _turmaRepositorio.GetTurmaById(turmaId);
            Disciplina disciplina = await _disciplinaRepositorio.GetEntityById(disciplinaId);
            Semestre semestre = await _semestreRepositorio.GetEntityById(semestreId);

            if (turma is null || disciplina is null || semestre is null)
            {
                return NotFound();
            }

            //verificar se usuário logado pode fazer a matricula
            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            Estudante estudante = await _estudanteRepositorio.GetById(loggedUser.EstudanteId)
                .Include(e => e.Matriculas).ThenInclude(m => m.Turma)
                .FirstOrDefaultAsync();
            
            if (estudante is not null)
            {

                //verificar se o estudante já está matriculado na turma
                MatriculaTurma matriculaTurma = await _matriculaTurmaRepositorio.GetByEstudanteAndTurma(estudante, turma);
                if (matriculaTurma is not null)
                {
                    TempData["ErrorMessageTemp"] = "Você já está matriculado nessa turma.";
                    return RedirectToAction("Details", new {  id = disciplinaId});
                }
                //verificar se o estudante não possui outra turma nesses horarios
                List<MatriculaTurma> matriculasSemestreAtual = await _matriculaTurmaRepositorio.GetByEstudanteAndSemestre(estudante, semestre);
                bool hasConflict = false;
                if(matriculasSemestreAtual.Count > 0)
                {
                    foreach (var matricula in matriculasSemestreAtual)
                    {
                        if (hasConflict == true) break;
                        foreach (var turmaHorario in turma.Horarios)
                        {
                            if (matricula.Turma.Horarios.Any(h => h.Horario.Equals(turmaHorario.Horario)))
                            {
                                hasConflict = true;
                                break;
                            }
                        }
                    }
                }

                //verificar se o estudante possui os pre requisitos
                if (turma.Disciplina.Requisitos.Count() > 0)
                {
                    if(matriculasSemestreAtual.Count > 0)
                    {
                        foreach (var preRequisito in turma.Disciplina.Requisitos)
                        {
                            if (estudante.Matriculas.Any(mt => mt.Turma.DisciplinaId == preRequisito.DisciplinaId))
                            {
                                continue;
                            }
                            else
                            {
                                TempData["ErrorMessageTemp"] = "Você não possui os pre requisitos para esta disciplina.";
                                return RedirectToAction("Details", new { id = disciplinaId });
                            }
                        }
                    }
                    TempData["ErrorMessageTemp"] = "Você não possui os pre requisitos necessários para esta disciplina.";
                    return RedirectToAction("Details", new { id = disciplinaId });
                }

                if (!hasConflict)
                {
                    await _matriculaTurmaRepositorio.MatricularEstudanteAsync(turma, estudante);
                    TempData["ErrorMessageTemp"] = "Matricula realizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                } else
                {
                    TempData["ErrorMessageTemp"] = "Você já possui uma matrícula neste(s) horário(s).";
                    return RedirectToAction("Details", new { id = disciplinaId });
                }
            }

            return NotFound();
        }

        // GET: MatriculaTurmas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matriculaTurma = await _context.MatriculaTurmas.FindAsync(id);
            if (matriculaTurma == null)
            {
                return NotFound();
            }
            ViewData["EstudanteId"] = new SelectList(_context.Estudantes, "Id", "Id", matriculaTurma.EstudanteId);
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "Id", "Id", matriculaTurma.TurmaId);
            return View(matriculaTurma);
        }

        // POST: MatriculaTurmas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Nota,Aprovado,TurmaId,EstudanteId")] MatriculaTurma matriculaTurma)
        {
            if (id != matriculaTurma.TurmaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matriculaTurma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatriculaTurmaExists(matriculaTurma.TurmaId))
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
            ViewData["EstudanteId"] = new SelectList(_context.Estudantes, "Id", "Id", matriculaTurma.EstudanteId);
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "Id", "Id", matriculaTurma.TurmaId);
            return View(matriculaTurma);
        }

        // GET: MatriculaTurmas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matriculaTurma = await _context.MatriculaTurmas
                .Include(m => m.Estudante)
                .Include(m => m.Turma)
                .FirstOrDefaultAsync(m => m.TurmaId == id);
            if (matriculaTurma == null)
            {
                return NotFound();
            }

            return View(matriculaTurma);
        }

        // POST: MatriculaTurmas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var matriculaTurma = await _context.MatriculaTurmas.FindAsync(id);
            _context.MatriculaTurmas.Remove(matriculaTurma);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatriculaTurmaExists(long id)
        {
            return _context.MatriculaTurmas.Any(e => e.TurmaId == id);
        }
    }
}
