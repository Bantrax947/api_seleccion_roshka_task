namespace Core.Domain.Entities
{
    public class HistorialTarea
    {
        public int Id { get; set; }
        public int TareaId { get; set; }
        public string EstadoAnterior { get; set; }
        public string EstadoNuevo { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}