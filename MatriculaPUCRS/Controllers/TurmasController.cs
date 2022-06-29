using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos;
using Persistencia.Interfaces.Repositorios;
using Microsoft.AspNetCore.Authorization;
using MatriculaPUCRS.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace MatriculaPUCRS.Controllers
{
    [Authorize(Roles = "Coordenador")]
    public class TurmasController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITurmaRepositorio _turmaRepositorio;
        private readonly IMatriculaTurmaRepositorio _matriculaTurmaRepositorio;
        private readonly IEstudanteRepositorio _estudanteRepositorio;
        private readonly ISemestreRepositorio _semestreRepositorio;
        private readonly IDisciplinaRepositorio _disciplinaRepositorio;
        public TurmasController(
            UserManager<ApplicationUser> userManager, 
            ITurmaRepositorio turmaRepositorio, 
            IMatriculaTurmaRepositorio matriculaTurmaRepositorio, 
            IEstudanteRepositorio estudanteRepositorio,
            ISemestreRepositorio semestreRepositorio,
            IDisciplinaRepositorio disciplinaRepositorio)
        {
            _userManager = userManager;
            _turmaRepositorio = turmaRepositorio;
            _matriculaTurmaRepositorio = matriculaTurmaRepositorio;
            _estudanteRepositorio = estudanteRepositorio;
            _semestreRepositorio = semestreRepositorio;
            _disciplinaRepositorio = disciplinaRepositorio;
        }

        // GET: Turmas
        public IActionResult Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TurmaSortParm"] = "Turma";
            ViewData["TituloSortParm"] = "Titulo";
            ViewData["HorarioSortParm"] = "Horario";
            ViewData["SemestreSortParm"] = "Semestre";
            IEnumerable<Turma> turmas = _turmaRepositorio.ListTurmasWithDisciplinaAndSemestreAndHorariosAndMatriculas();
            switch (sortOrder)
            {
                case "Turma":
                    turmas = turmas.OrderBy(t => t.Id);
                    break;
                case "Titulo":
                    turmas = turmas.OrderBy(t => t.Disciplina.Nome);
                    break;
                case "Horario":
                    turmas = turmas.OrderBy(t => t.Horarios.OrderBy(h => h.Horario).First().Horario);
                    break;
                case "Semestre":
                    turmas = turmas.OrderBy(t => t.Semestre.Titulo).ThenBy(t => t.Id);
                    break;
                default:
                    turmas = turmas.OrderBy(t => t.Id);
                    break;
            }
            return View(turmas);
        }

        // GET: Turmas/Create
        public IActionResult Create()
        {
            var turma = new Turma();
            PopularHorariosTurma(turma);
            ViewBag.Disciplinas = new SelectList(_disciplinaRepositorio.List().Result, nameof(Disciplina.Id), nameof(Disciplina.NomeParaLista));
            ViewBag.Semestres = new SelectList(_semestreRepositorio.List().Result, nameof(Semestre.Id), nameof(Semestre.Titulo));
            return View();
        }

        // POST: Turmas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroDeVagas,DisciplinaId,SemestreId")] Turma turma, string[] horariosSelecionados)
        {
            if (horariosSelecionados is not null)
            {
                turma.Horarios = await _turmaRepositorio.GetHorarios().Where(hg => horariosSelecionados.Contains(hg.Horario)).ToListAsync();
            }

            if (ModelState.IsValid)
            {
                await _turmaRepositorio.Add(turma);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Disciplinas = new SelectList(await _disciplinaRepositorio.List(), nameof(Disciplina.Id), nameof(Disciplina.NomeParaLista), turma.DisciplinaId);
            ViewBag.Semestres = new SelectList(await _semestreRepositorio.List(), nameof(Semestre.Id), nameof(Semestre.Titulo), turma.SemestreId);
            return View(turma);
        }

        // GET: Turmas/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Turma turma = await _turmaRepositorio.GetTurmaByIdWithEstudantesAsync((long) id);

            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

        // GET: Turmas/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _turmaRepositorio.GetTurmaByIdAsync((long) id);
            if (turma == null)
            {
                return NotFound();
            }

            // Verifica se a turma possui alunos matriculados
            if (turma.Matriculas.Any())
            {
                return BadRequest("Não é possível editar uma turma com estudantes matriculados.");
                //ModelState.AddModelError("", "Não é possível editar uma turma com estudantes matriculados.");
                //return RedirectToAction(nameof(Index));
                //return NotFound();
            }

            ViewData["Disciplinas"] = new SelectList(_disciplinaRepositorio.List().Result, nameof(Disciplina.Id), nameof(Disciplina.NomeParaLista), turma.DisciplinaId);
            ViewData["Semestres"] = new SelectList(_semestreRepositorio.List().Result, nameof(Semestre.Id), nameof(Semestre.Titulo), turma.SemestreId);
            PopularHorariosTurma(turma);
            return View(turma);
        }

        // POST: Turmas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,NumeroDeVagas,DisciplinaId,SemestreId")] Turma turma, string[] horariosSelecionados)
        {
            if (id != turma.Id)
            {
                return NotFound();
            }

            // Verifica se foi removida
            var turmaContext = await _turmaRepositorio.GetTurmaByIdAsync((long)id);
            if (turmaContext == null)
            {
                return NotFound();
            }

            // Verifica se a turma possui alunos matriculados
            if (turmaContext.Matriculas.Any())
            {
                return NotFound();
            }

            if (horariosSelecionados is not null)
            {
                turma.Horarios = await _turmaRepositorio.GetHorarios().Where(hg => horariosSelecionados.Contains(hg.Horario)).ToListAsync();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _turmaRepositorio.Update(turma);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _turmaRepositorio.EntityExistsById(turma.Id))
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


            ViewData["Disciplinas"] = new SelectList(await _disciplinaRepositorio.List(), nameof(Disciplina.Id), nameof(Disciplina.NomeParaLista), turma.DisciplinaId);
            ViewData["Semestres"] = new SelectList(await _semestreRepositorio.List(), nameof(Semestre.Id), nameof(Semestre.Titulo), turma.SemestreId);
            return View(turma);
        }

        // GET: Turmas/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _turmaRepositorio.GetTurmaByIdAsync((long) id);
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
            var turma = await _turmaRepositorio.GetEntityById(id);
            await _turmaRepositorio.Delete(turma);
            return RedirectToAction(nameof(Index));
        }

        private void PopularHorariosTurma(Turma turma)
        {
            //var horarios = _turmaRepositorio.GetHorarios();
            //var horariosTurma = new HashSet<long>(turma.Horarios.Select(h => h.Id));
            //var viewModel = new List<HorariosSelecionado>();
            //foreach (var horario in horarios)
            //{
            //    viewModel.Add(new HorariosSelecionado
            //    {
            //        Id = horario.Id,
            //        Horario = horario.Horario,
            //        Selecionado = horariosTurma.Contains(horario.Id)
            //    });
            //}
            var horarios = _turmaRepositorio.GetHorarios().Select(hg => 
                new HorariosSelecionado
                {
                    Id = hg.Id,
                    Horario = hg.Horario,
                    Selecionado = turma.Horarios.Contains(hg),
                }).ToList();
            ViewData["Horarios"] = horarios;
        }
    }
}
