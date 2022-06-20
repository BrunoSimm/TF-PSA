using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.AspNetCore.Authorization;
using Persistencia.Interfaces.Repositorios;

namespace MatriculaPUCRS.Controllers
{
    [Authorize(Roles ="Coordenador")]
    public class SemestresController : Controller
    {
        private readonly MatriculaContext _context;
        private readonly ISemestreRepositorio _semestreRepositorio;

        public SemestresController(
            MatriculaContext context, 
            ISemestreRepositorio semestreRepositorio)
        {
            _context = context;
            _semestreRepositorio = semestreRepositorio;
        }

        // GET: Semestres
        public async Task<IActionResult> Index()
        {
            IOrderedEnumerable<Semestre> semestres = await _semestreRepositorio.ListSemestresWithTurmas()
                .ContinueWith(list => list.Result.OrderBy(s => s.Titulo));

            return View(semestres);
        }

        // GET: Semestres/Create
        public IActionResult Create()
        {
            return View(new Semestre());
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
                await _semestreRepositorio.Add(semestre);
                return RedirectToAction(nameof(Index));
            }
            return View(semestre);
        }

        // GET: Semestres/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Semestre semestre = await _semestreRepositorio.GetSemestreWithTurmasById((long) id);
            if (semestre == null)
            {
                return NotFound();
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

            Semestre semestre = await _semestreRepositorio.GetEntityById((long) id);
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
                    await _semestreRepositorio.Update(semestre);
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

            Semestre semestre = await _semestreRepositorio.GetEntityById((long) id);
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
            Semestre semestre = await _semestreRepositorio.GetEntityById(id);
            await _semestreRepositorio.Delete(semestre);
            return RedirectToAction(nameof(Index));
        }

        private bool SemestreExists(long id)
        {
            return _context.Semestres.Any(e => e.Id == id);
        }
    }
}
