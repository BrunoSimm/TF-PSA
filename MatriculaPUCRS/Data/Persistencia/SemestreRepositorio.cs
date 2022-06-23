using Entidades.Modelos;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaPUCRS.Data.Persistencia
{
    public class SemestreRepositorio : CrudEF<Semestre, MatriculaContext>, ISemestreRepositorio
    {
        private readonly MatriculaContext _matriculaContext;
        public SemestreRepositorio(MatriculaContext context) : base(context)
        {
            _matriculaContext = context;
        }

        public async Task<Semestre> GetSemestreAtualAsync()
        {
            return await _matriculaContext.Semestres
                .Where(s => DateTime.Now >= s.DataInicial && DateTime.Now <= s.DataFinal)
                .OrderBy(s => s.DataFinal)
                .FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<Semestre>> ListSemestresWithTurmas()
        {
            return await _matriculaContext.Semestres
                .Include(s => s.Turmas)
                    .ThenInclude(t => t.Matriculas)
                        .ThenInclude(mt => mt.Estudante)
                .ToListAsync();
        }

        public async Task<Semestre> GetSemestreWithTurmasById(long Id)
        {
            return await _matriculaContext.Semestres
                .Include(s => s.Turmas)
                    .ThenInclude(t => t.Matriculas)
                        .ThenInclude(mt => mt.Estudante)
                .Include(s => s.Turmas)
                    .ThenInclude(t => t.Disciplina)
                .Include(s => s.Turmas)
                    .ThenInclude(t => t.Horarios)
                .FirstOrDefaultAsync(s => s.Id == Id);
        }
    }
}
