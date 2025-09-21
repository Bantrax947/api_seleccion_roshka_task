using Core.Contracts.Request;
using Core.Contracts.Resposes;
using Core.Domain.Entities;

namespace Core.Interfaces
{
    public interface ITaskService
    {
        Task<int> InsertarTarea(TareaRequest request);

        Task<PagedResult<Tarea>> ObtenerTareasPaginadas(PagedRequest request);

        Task<Tarea> ObtenerTareaId(int id);

        Task ActualizarTarea(int id, ActualizarTareaRequest request);

        Task EliminarTarea(int id);
    }
}