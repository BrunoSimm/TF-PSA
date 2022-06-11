namespace Entidades.Modelos
{
    public class MatriculaTurma
    {
        // PK EstudanteId&TurmaId
        public float Nota { get; set; }
        public bool Aprovado { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }
        public long EstudanteId { get; set; }
        public Estudante Estudante { get; set; }
        public EstadoMatriculaTurmaEnum Estado { get; set; }
    }
}
