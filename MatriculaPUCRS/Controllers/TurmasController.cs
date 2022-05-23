﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos;
using Infraestrutura.Data;

namespace MatriculaPUCRS.Controllers
{
    public class TurmasController : Controller
    {
        private readonly MatriculaContext _context;

        public TurmasController(MatriculaContext context)
        {
            _context = context;
        }

        // GET: Turmas
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "titulo_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["HorarioSortParm"] = sortOrder == "Horario" ? "horario_desc" : "Horario";
            var turmas = _context.Turmas.Include(t => t.Disciplina).Include(t => t.Semestre).Include(t => t.Horarios).AsQueryable();
            switch (sortOrder)
            {
                case "titulo_desc":
                    turmas = turmas.OrderBy(t => t.Disciplina.Nome);
                    break;
                case "horario_desc":
                    turmas = turmas.OrderBy(t => t.Horarios.OrderBy(h => h.Horario).First().Horario);
                    break;
                case "id_desc":
                    turmas = turmas.OrderByDescending(t => t.Id);
                    break;
                default:
                    turmas = turmas.OrderBy(t => t.Id);
                    break;
            } 
            return View(await turmas.AsNoTracking().ToListAsync());
        }

        // GET: Turmas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas
                .Include(t => t.Disciplina)
                .Include(t => t.Semestre)
                .Include(t => t.Horarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

        // GET: Turmas/Create
        public IActionResult Create()
        {
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Id");
            ViewData["SemestreId"] = new SelectList(_context.Semestres, "Id", "Id");
            return View();
        }

        // POST: Turmas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroDeVagas,DisciplinaId,SemestreId")] Turma turma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Id", turma.DisciplinaId);
            ViewData["SemestreId"] = new SelectList(_context.Semestres, "Id", "Id", turma.SemestreId);
            return View(turma);
        }

        // GET: Turmas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
            {
                return NotFound();
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Id", turma.DisciplinaId);
            ViewData["SemestreId"] = new SelectList(_context.Semestres, "Id", "Id", turma.SemestreId);
            return View(turma);
        }

        // POST: Turmas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,NumeroDeVagas,DisciplinaId,SemestreId")] Turma turma)
        {
            if (id != turma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurmaExists(turma.Id))
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
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Id", turma.DisciplinaId);
            ViewData["SemestreId"] = new SelectList(_context.Semestres, "Id", "Id", turma.SemestreId);
            return View(turma);
        }

        // GET: Turmas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas
                .Include(t => t.Disciplina)
                .Include(t => t.Semestre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

        // POST: Turmas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurmaExists(long id)
        {
            return _context.Turmas.Any(e => e.Id == id);
        }
    }
}