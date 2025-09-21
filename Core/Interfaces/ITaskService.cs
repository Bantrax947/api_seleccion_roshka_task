using Core.Contracts.Request;
using Core.Contracts.Resposes;
using Core.Domain.Entities;

namespace Core.Interfaces
{
    public interface ITaskService
    {
        Task<int> InsertarTarea(Tarea tarea);

        Task<PagedResult<Tarea>> ObtenerTarea(int pagina, int cantidadPorPagina);

        Task<Tarea?> ObtenerTareaId(int id);

        Task<bool> ActualizarTarea(Tarea tarea);

        Task<bool> EliminarTarea(int id);
    }
}