﻿using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface IDisciplinaRepositorio : ICrud<Disciplina>
    {
        public Task<Disciplina> GetDisciplinaByIdWithMatriculasAndSemestre(long id);
        public Task<List<Disciplina>> GetDisciplinasFromCurrentSemester();
        public Task<List<Disciplina>> GetDisciplinasFromSemestre(long estudanteId, long semestreId);
        public Task<Curriculo> GetDisciplinasFromCurriculoId(long id);
    }
}
