namespace Core.Contracts.Request
{
    public class TareaRequest : BaseTareaRequest
    {
        public DateTime FechaVencimiento { get; set; }
        public string? Descripcion { get; set; }
    }
}