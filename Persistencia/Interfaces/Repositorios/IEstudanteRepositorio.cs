﻿using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Interfaces.Repositorios
{
    public interface IEstudanteRepositorio : ICrud<Estudante>
    {
        IQueryable<Estudante> GetById(long id);
    }
}