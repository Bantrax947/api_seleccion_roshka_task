namespace Core.Contracts.Request
{
    public class BaseTareaRequest
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int Prioridad { get; set; }
    }
}