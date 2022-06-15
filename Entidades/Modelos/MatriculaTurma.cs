﻿using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos
{
    public class MatriculaTurma
    {
        // PK EstudanteId&TurmaId
        [Range(0, 10, ErrorMessage = "A Nota deve estar entre 0 e 10.")]
        [RegularExpression(@"^\d+(\.\d)?$", ErrorMessage = "Nota deve ter até uma casa decimal.")]
        public float Nota { get; set; } // trocar para nullable?
        public bool Aprovado { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
        public long EstudanteId { get; set; }
        public Estudante Estudante { get; set; }
        public EstadoMatriculaTurmaEnum Estado { get; set; } = EstadoMatriculaTurmaEnum.MATRICULADO;
    }
}
