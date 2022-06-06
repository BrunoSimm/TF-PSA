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

namespace MatriculaPUCRS.Controllers
{
    public class MatriculasController : Controller
    {
        private readonly MatriculaContext _context;
        private readonly IDisciplinaRepositorio _disciplinaRepositorio;
        private readonly ITurmaRepositorio _turmaRepositorio;

        public MatriculasController(MatriculaContext context, IDisciplinaRepositorio disciplinaRepositorio, ITurmaRepositorio turmaRepositorio)
        {
            _context = context;
            _disciplinaRepositorio = disciplinaRepositorio;
            _turmaRepositorio = turmaRepositorio;
        }

        // GET: Matriculas
        public async Task<IActionResult> Index()
        {
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

            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "titulo_asc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["HorarioSortParm"] = sortOrder == "Horario" ? "horario_desc" : "Horario";
            var turmas = _turmaRepositorio
                .ListTurmasWithDisciplinaAndSemestreAndHorariosAsQueryable()
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
            return View(await turmas.AsNoTracking().ToListAsync());
        }

        // GET: MatriculaTurmas/Create
        public IActionResult Create()
        {
            ViewData["EstudanteId"] = new SelectList(_context.Estudantes, "Id", "Id");
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "Id", "Id");
            return View();
        }

        // POST: MatriculaTurmas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Matricular([Bind("Nota,Aprovado,TurmaId,EstudanteId")] MatriculaTurma matriculaTurma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(matriculaTurma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstudanteId"] = new SelectList(_context.Estudantes, "Id", "Id", matriculaTurma.EstudanteId);
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "Id", "Id", matriculaTurma.TurmaId);
            return View(matriculaTurma);
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
