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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDisciplinaRepositorio _disciplinaRepositorio;
        private readonly ITurmaRepositorio _turmaRepositorio;
        private readonly IMatriculaTurmaRepositorio _matriculaTurmaRepositorio;
        private readonly IEstudanteRepositorio _estudanteRepositorio;
        private readonly ISemestreRepositorio _semestreRepositorio;

        public MatriculasController(
            UserManager<ApplicationUser> userManager,
            IDisciplinaRepositorio disciplinaRepositorio,
            ITurmaRepositorio turmaRepositorio,
            IMatriculaTurmaRepositorio matriculaTurmaRepositorio,
            IEstudanteRepositorio estudanteRepositorio,
            ISemestreRepositorio semestreRepositorio)
        {
            _userManager = userManager;
            _disciplinaRepositorio = disciplinaRepositorio;
            _turmaRepositorio = turmaRepositorio;
            _matriculaTurmaRepositorio = matriculaTurmaRepositorio;
            _estudanteRepositorio = estudanteRepositorio;
            _semestreRepositorio = semestreRepositorio;
        }

        // GET: Matriculas
        public async Task<IActionResult> Index(string sortOrder)
        {
            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            if (loggedUser.EstudanteId is null)
            {
                return NotFound();
            }

            Estudante estudante = await _estudanteRepositorio.GetByIdAsync(loggedUser.EstudanteId);

            Semestre semestre = await _semestreRepositorio.GetSemestreAtualAsync();

            if (semestre is null || estudante is null)
            {
                return NotFound();
            }

            ViewBag.MatriculaSuccessMessage = TempData["SuccessMessageTemp"];
            ViewBag.Semestre = semestre;

            IEnumerable<MatriculaTurma> matriculasTurmas = estudante.Matriculas.Where(mt => mt.Turma.SemestreId.Equals(semestre.Id));

            return View(matriculasTurmas);
        }


        // GET: Matriculas/Disciplinas
        public async Task<IActionResult> Disciplinas()
        {
            ViewBag.MatriculaSuccessMessage = TempData["ErrorMessageTemp"];

            Semestre semestre = await _semestreRepositorio.GetSemestreAtualAsync();

            IEnumerable<Disciplina> disciplinas = await _disciplinaRepositorio.GetDisciplinasFromCurrentSemester();
            disciplinas = disciplinas.Distinct();

            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            Estudante estudante = await _estudanteRepositorio.GetByIdAsync(loggedUser.EstudanteId);

            IEnumerable<Disciplina> disciplinasCursadas = estudante.Matriculas
                .Where(mt => mt.Estado == (EstadoMatriculaTurmaEnum.APROVADO ^ EstadoMatriculaTurmaEnum.CURSANDO))
                .Select(mt => mt.Turma.Disciplina).Distinct();

            disciplinas = disciplinas.Where(d => !disciplinasCursadas.Contains(d));

            //Remove as disciplinas que o estudante não possui pre-requisitos.
            List<Disciplina> disciplinasComPreReq = new List<Disciplina>();
            foreach (var disciplina in disciplinas)
            {
                if (disciplina.Requisitos.Any())
                {
                    int hasRequirements = 0;
                    foreach (var preRequisito in disciplina.Requisitos)
                    {
                        if (estudante.Matriculas.Any(mt => mt.Turma.DisciplinaId == preRequisito.Id && mt.Aprovado == true))
                        {
                            hasRequirements++;
                        }
                    }
                    //Possui todos os pre-requisitos.
                    if (hasRequirements == disciplina.Requisitos.Count()) {
                        disciplinasComPreReq.Add(disciplina);
                    } 
                }
                else
                {
                    disciplinasComPreReq.Add(disciplina);
                }
            }


            //IEnumerable<MatriculaTurma> turmasMatriculado = estudante.Matriculas.Where(mt => mt.Turma.SemestreId.Equals(semestre.Id));

            //List<Disciplina> disciplinas2 = await _disciplinaRepositorio.GetDisciplinasFromSemester(estudante.Id, semestre.Id);

            //IQueryable<Turma> turmas = _turmaRepositorio
            //    .ListTurmasWithDisciplinaAndSemestreAndHorariosAndMatriculasAsQueryable()
            //    .Where(t => t.DisciplinaId == id)
            //    .Where(t => DateTime.Now >= t.Semestre.DataInicial && DateTime.Now <= t.Semestre.DataFinal);

            ViewBag.TurmasMatriculado = estudante.Matriculas.Where(mt => mt.Turma.SemestreId.Equals(semestre.Id)).Select(mt => mt.Turma);
            ViewBag.DisciplinasMatriculado = estudante.Matriculas.Where(mt => mt.Turma.SemestreId.Equals(semestre.Id)).Select(mt => mt.Turma.Disciplina).Distinct();
            ViewBag.Semestre = semestre;
            return View(disciplinasComPreReq);
        }

        // GET: Matriculas/GradeDeHorario
        public async Task<IActionResult> GradeDeHorario()
        {
            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            Estudante estudante = await _estudanteRepositorio.GetByIdAsync(loggedUser.EstudanteId);

            Semestre semestre = await _semestreRepositorio.GetSemestreAtualAsync();

            if (semestre is null || estudante is null)
            {
                return NotFound();
            }

            ViewBag.MatriculaSuccessMessage = TempData["SuccessMessageTemp"];
            ViewBag.Semestre = semestre;

            IEnumerable<MatriculaTurma> matriculasTurmas = estudante.Matriculas.Where(mt => mt.Turma.SemestreId.Equals(semestre.Id));

            return View(matriculasTurmas);
        }

        // GET: Matriculas/Comprovante
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comprovante()
        {
            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            Estudante estudante = await _estudanteRepositorio.GetByIdAsync(loggedUser.EstudanteId);

            var semestre = await _semestreRepositorio.GetSemestreAtualAsync();

            if (semestre is null || estudante is null)
            {
                return NotFound();
            }

            List<MatriculaTurma> matriculasTurmas = await _matriculaTurmaRepositorio.GetByEstudanteAndSemestre(estudante, semestre);

            return View(matriculasTurmas);
        }

        // GET: Matriculas/Details/5
        // Lista Turmas da Disciplina id
        public async Task<IActionResult> Details(long? id, string sortOrder)
        {
            if (id == null)
            {
                return NotFound();
            }

            Semestre semestre = await _semestreRepositorio.GetSemestreAtualAsync();

            if (semestre is null)
            {
                return NotFound();
            }

            ViewBag.ErrorMessage = TempData["ErrorMessageTemp"];
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParm"] = "Id";
            ViewData["HorarioSortParm"] = "Horario";

            Disciplina disciplina = await _disciplinaRepositorio.GetDisciplinaByIdWithMatriculasAndSemestre((long) id);

            IEnumerable<Turma> turmas = disciplina.Turmas.Where(t => t.SemestreId == semestre.Id);

            if (!turmas.Any())
            {
                return NotFound();
            }

            ViewBag.Semestre = semestre;
            ViewBag.Disciplina = disciplina;

            switch (sortOrder)
            {
                case "Id":
                    turmas = turmas.OrderBy(t => t.Id);
                    break;
                case "Horario":
                    turmas = turmas.OrderBy(t => t.Horarios.OrderBy(h => h.Horario).First().Horario);
                    break;
                default:
                    turmas = turmas.OrderBy(t => t.Id);
                    break;
            }

            return View(turmas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Matricular(long turmaId, long disciplinaId, long semestreId)
        {
            Turma turma = await _turmaRepositorio.GetTurmaByIdAsync(turmaId);
            Disciplina disciplina = await _disciplinaRepositorio.GetEntityById(disciplinaId);
            Semestre semestre = await _semestreRepositorio.GetEntityById(semestreId);

            if (turma is null || disciplina is null || semestre is null)
            {
                return NotFound();
            }

            //verificar se usuário logado pode fazer a matricula
            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            Estudante estudante = await _estudanteRepositorio.GetByIdAsync(loggedUser.EstudanteId);

            if (estudante is null)
            {
                return NotFound();
            }

            //verificar se o estudante possui os pre requisitos
            if (turma.Disciplina.Requisitos.Any())
            {
                foreach (var preRequisito in turma.Disciplina.Requisitos)
                {
                    if (!estudante.Matriculas.Any(mt => mt.Turma.DisciplinaId == preRequisito.Id && mt.Estado == EstadoMatriculaTurmaEnum.APROVADO))
                    {
                        TempData["ErrorMessageTemp"] = $"Você não cumpre o pré-requisito '{preRequisito.NomeParaLista}' para se matricular nesta disciplina.";
                        return RedirectToAction("Details", new { id = disciplinaId });
                    }
                }
            }

            //verificar se existem vagas disponíveis na turma
            if (turma.VagasRemanescentes == 0)
            {
                TempData["ErrorMessageTemp"] = "Não há vagas disponíveis para esta turma.";
                return RedirectToAction("Details", new { id = disciplinaId });
            }

            //verificar se o estudante já cursou esta disciplina
            if (estudante.Matriculas.Any(m => m.Turma.DisciplinaId == disciplinaId && m.Aprovado == true))
            {
                TempData["ErrorMessageTemp"] = "Você já cursou e foi aprovado nesta disciplina.";
                return RedirectToAction("Details", new { id = disciplinaId });
            }

            //verificar se o estudante já está matriculado nesta disciplina no semestre atual.
            List<MatriculaTurma> matriculaDisciplina = await _matriculaTurmaRepositorio.GetByEstudanteAndSemestre(estudante, semestre);
            if (matriculaDisciplina.Count > 0)
            {
                if (matriculaDisciplina.Any(md => md.Turma.Id == turma.Id))
                {
                    TempData["ErrorMessageTemp"] = "Você já está matriculado nessa turma.";
                    return RedirectToAction("Details", new { id = disciplinaId });
                }

                if (matriculaDisciplina.Any(md => md.Turma.DisciplinaId == disciplina.Id))
                {
                    TempData["ErrorMessageTemp"] = "Você já está matriculado nesta disciplina neste semestre.";
                    return RedirectToAction("Details", new { id = disciplinaId });
                }
            }

            //verificar se o estudante não possui outra turma nos horarios desta Turma
            List<MatriculaTurma> matriculasSemestreAtual = await _matriculaTurmaRepositorio.GetByEstudanteAndSemestre(estudante, semestre);
            bool hasConflict = false;
            if (matriculasSemestreAtual.Count > 0)
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

            if (hasConflict)
            {
                TempData["ErrorMessageTemp"] = "Você já está matriculado em outra turma nos mesmos horários.";
                return RedirectToAction("Details", new { id = disciplinaId });
            }

            //Realiza a Matricula.
            await _matriculaTurmaRepositorio.MatricularEstudanteAsync(turma, estudante);
            TempData["SuccessMessageTemp"] = $"Matrícula na turma {turma.Id} realizada com sucesso!";
            return RedirectToAction("Index");
        }

        // GET: Matricula/Turma/5
        public async Task<IActionResult> Turma(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            Estudante estudante = await _estudanteRepositorio.GetByIdAsync(loggedUser.EstudanteId);

            Semestre semestre = await _semestreRepositorio.GetSemestreAtualAsync();

            if (semestre is null || estudante is null)
            {
                return NotFound();
            }

            Turma turma = await _turmaRepositorio.GetTurmaByIdAsync((long) id);

            if (turma == null)
            {
                return NotFound();
            }

            //ViewBag.CancelarMatriculaStatus = TempData["CancelarMatriculaStatus"];

            //Verifica se o usuário logado está matriculado nesta turma.
            IEnumerable<MatriculaTurma> matriculasTurmas = estudante.Matriculas.Where(mt => mt.Turma.SemestreId.Equals(semestre.Id));
            MatriculaTurma matriculaTurma = matriculasTurmas.FirstOrDefault(mt => mt.TurmaId == turma.Id);

            ViewBag.Matricula = matriculaTurma;
            ViewBag.IsMatriculated = matriculaTurma is not null && matriculaTurma.Estado.Equals(EstadoMatriculaTurmaEnum.MATRICULADO);
            ViewBag.Estado = await _estudanteRepositorio.GetStatusDisciplina(estudante.Id, turma.DisciplinaId);

            return View(turma);
        }

        // POST: Turmas/CancelarMatricula/5
        [HttpPost]
        [ActionName("CancelarMatricula")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarMatricula(long id)
        {
            //Verifica se o usuário logado está matriculado nesta turma.
            ApplicationUser loggedUser = await _userManager.GetUserAsync(User);
            Estudante estudante = await _estudanteRepositorio.GetByIdAsync(loggedUser.EstudanteId);

            Semestre semestre = await _semestreRepositorio.GetSemestreAtualAsync();

            Turma turma = await _turmaRepositorio.GetEntityById(id);

            if (semestre is null || estudante is null || turma is null)
            {
                return NotFound();
            }

            MatriculaTurma matriculaTurma = await _matriculaTurmaRepositorio.GetEntityById(turma.Id, estudante.Id);
            if (matriculaTurma is null)
            {
                return NotFound();
            }

            await _matriculaTurmaRepositorio.Delete(matriculaTurma);
            TempData["CancelarMatriculaStatus"] = "Matrícula cancelada com sucesso.";
            return RedirectToAction(nameof(Turma), new { id = turma.Id });
        }

        // COORDENADOR PODE EDITAR E EXCLUIR MATRICULAS?
        //// GET: Matricula/Edit/5
        //public async Task<IActionResult> Edit(long? turmaId, long estudanteId)
        //{
        //    if (turmaId == null)
        //    {
        //        return NotFound();
        //    }

        //    var matriculaTurma = await _matriculaTurmaRepositorio.GetEntityById((long) turmaId, estudanteId);
        //    if (matriculaTurma == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["Estudantes"] = new SelectList(await _estudanteRepositorio.List(), nameof(Estudante.Id), nameof(Estudante.NumeroMatricula), matriculaTurma.EstudanteId);
        //    ViewData["Turmas"] = new SelectList(await _turmaRepositorio.List(), nameof(Entidades.Modelos.Turma.Id), nameof(Entidades.Modelos.Turma.Id), matriculaTurma.TurmaId);
        //    return View(matriculaTurma);
        //}

        //// POST: Matricula/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("Nota,Aprovado,TurmaId,EstudanteId")] MatriculaTurma matriculaTurma)
        //{
        //    if (id != matriculaTurma.TurmaId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await _matriculaTurmaRepositorio.Update(matriculaTurma);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await _matriculaTurmaRepositorio.MatriculaTurmaExists(matriculaTurma.TurmaId, matriculaTurma.EstudanteId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Estudantes"] = new SelectList(await _estudanteRepositorio.List(), nameof(Estudante.Id), nameof(Estudante.NumeroMatricula), matriculaTurma.EstudanteId);
        //    ViewData["Turmas"] = new SelectList(await _turmaRepositorio.List(), nameof(Entidades.Modelos.Turma.Id), nameof(Entidades.Modelos.Turma.Id), matriculaTurma.TurmaId);
        //    return View(matriculaTurma);
        //}

        //// GET: Matricula/Delete/5
        //public async Task<IActionResult> Delete(long? turmaId, long estudanteId)
        //{
        //    if (turmaId == null)
        //    {
        //        return NotFound();
        //    }

        //    var matriculaTurma = await _matriculaTurmaRepositorio.GetByEstudanteAndTurma((long) turmaId, estudanteId);
        //    if (matriculaTurma == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(matriculaTurma);
        //}

        //// POST: Matricula/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long turmaId, long estudanteId)
        //{
        //    var matriculaTurma = await _matriculaTurmaRepositorio.GetEntityById(turmaId, estudanteId);
        //    await _matriculaTurmaRepositorio.Delete(matriculaTurma);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
