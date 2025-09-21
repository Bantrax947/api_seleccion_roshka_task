using Core.Contracts.Resposes;
using Core.Domain.Entities;

namespace Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<int> InsertarTarea(Tarea tarea);

        Task<PagedResult<Tarea>> ObtenerTarea(int pagina, int cantidadPorPagina);

        Task<Tarea?> ObtenerTareaId(int id);

        Task<bool> ActualizarTarea(Tarea tarea);

        Task<Tarea> ObtenerPorId(int id);

        Task<bool> ExisteTituloEnOtraTarea(string titulo, int id);

        Task<bool> EliminarTarea(int id);

    }
}