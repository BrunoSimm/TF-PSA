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

namespace MatriculaPUCRS.Controllers
{
    [Authorize(Roles = "Coordenador")]
    public class RelatoriosController : Controller
    {
        private readonly MatriculaContext _context;

        public RelatoriosController(MatriculaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexCursos()
        {
            return View(await _context.Curriculos.ToListAsync());
        }

        public async Task<IActionResult> IndexAlunos()
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
            ViewBag.Cursos = new SelectList(cursos, nameof(Curriculo.Id), nameof(Curriculo.NomeDoCurso), (long) id);

            var nomeDoCurso = _context.Curriculos.Find((long) id);
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

                if(matriculas.Any())
                {
                    qtdAlunosMatriculados += 1;
                    foreach (var matricula in matriculas)
                    {
                        totalDeCreditosMatriculados += int.Parse(matricula.Turma.Disciplina.Codigo.Substring(6));
                    }
                }
            }

            relatorio.PercentualDeMatriculas = (double) qtdAlunosMatriculados / relatorio.QuantidadeDeAlunos;

            relatorio.MediaDeCreditosPorAluno = (double) totalDeCreditosMatriculados / qtdAlunosMatriculados;

            return View(relatorio);
        }

        public IActionResult Alunos(long? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            RelatorioAlunosViewModel relatorio = new RelatorioAlunosViewModel() {
                Alunos = new List<AlunoViewModel>(),
                NomeDoCurso = _context.Curriculos.Find(id).NomeParaLista
            };

            var estudantesDoCurso = _context.Estudantes.Include(e => e.Curriculo)
                .Include(e => e.Matriculas)
                .ThenInclude(m => m.Turma)
                .ThenInclude(t => t.Disciplina)
                .Where(e => e.Curriculo.Id == id);

            foreach(var estudante in estudantesDoCurso)
            {
                AlunoViewModel aluno = new AlunoViewModel();
                aluno.NomeDoAluno = estudante.Nome;
                float notas = 0;
                int qtdDisciplinasCursadas = 0;

                foreach (var matricula in estudante.Matriculas)
                {
                    if (matricula.Estado == EstadoMatriculaTurmaEnum.APROVADO 
                        || matricula.Estado == EstadoMatriculaTurmaEnum.REPROVADO)
                    {
                        notas +=  matricula.Nota ?? 0;
                        qtdDisciplinasCursadas++;

                        if (matricula.Estado == EstadoMatriculaTurmaEnum.APROVADO)
                            aluno.NumeroDeCreditosComAprovacao += int.Parse(matricula.Turma.Disciplina.Codigo.Substring(6));
                    }
                }
                if(qtdDisciplinasCursadas > 0)
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

        public IActionResult Disciplinas(long id)
        {
            RelatorioDisciplinasViewModel relatorio = new RelatorioDisciplinasViewModel();

            return View(relatorio);
        }
    }
}
