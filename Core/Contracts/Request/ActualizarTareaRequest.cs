namespace Core.Contracts.Request
{
    public class ActualizarTareaRequest : BaseTareaRequest
    {
        public DateTime? FechaVencimiento { get; set; }
        public string? Descripcion { get; set; }
    }
}