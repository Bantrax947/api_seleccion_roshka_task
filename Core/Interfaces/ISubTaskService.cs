using Core.Contracts.Request;
using Core.Contracts.Resposes;
using Core.Domain.Entities;

namespace Core.Interfaces
{
    public interface  ISubTaskService
    {
        Task<int?> InsertarSubTarea(int tareaId, SubTareaRequest request);

        Task<PagedResult<SubTarea>> ObtenerSubTareasPaginadas(int tareaId, int page, int limit, string? order);

        Task<SubTarea> ObtenerSubTareaPorId(int tareaId, int subTareaId);

        Task ActualizarSubTarea(int tareaId, int subTareaId, ActualizarSubTareaRequest request);

        Task EliminarSubTarea(int tareaId, int subTareaId);
    }
}