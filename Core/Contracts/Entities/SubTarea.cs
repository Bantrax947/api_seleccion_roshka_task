namespace Core.Domain.Entities
{
    public class SubTarea
    {
        public int Id { get; set; }
        public int TareaId { get; set; }
        public string Titulo { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}