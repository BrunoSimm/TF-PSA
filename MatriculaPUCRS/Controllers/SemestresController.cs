using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Authorization;

namespace MatriculaPUCRS.Controllers
{
    //[Authorize(Roles ="Coordenador")]
    public class SemestresController : Controller
    {
        private readonly MatriculaContext _context;

        public SemestresController(MatriculaContext context)
        {
            _context = context;
        }

        // GET: Semestres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Semestres.ToListAsync());
        }

        // GET: Semestres/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semestre = await _context.Semestres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semestre == null)
            {
                return NotFound();
            }

            return View(semestre);
        }

        // GET: Semestres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Semestres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,DataInicial,DataFinal")] Semestre semestre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(semestre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(semestre);
        }

        // GET: Semestres/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semestre = await _context.Semestres.FindAsync(id);
            if (semestre == null)
            {
                return NotFound();
            }
            return View(semestre);
        }

        // POST: Semestres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Titulo,DataInicial,DataFinal")] Semestre semestre)
        {
            if (id != semestre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(semestre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemestreExists(semestre.Id))
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
            return View(semestre);
        }

        // GET: Semestres/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semestre = await _context.Semestres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semestre == null)
            {
                return NotFound();
            }

            return View(semestre);
        }

        // POST: Semestres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var semestre = await _context.Semestres.FindAsync(id);
            _context.Semestres.Remove(semestre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SemestreExists(long id)
        {
            return _context.Semestres.Any(e => e.Id == id);
        }
    }
}
