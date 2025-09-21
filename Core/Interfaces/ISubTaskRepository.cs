using Core.Contracts.Resposes;
using Core.Domain.Entities;

namespace Core.Interfaces
{
    public interface ISubTaskRepository
    {
        Task<int> InsertarSubTarea(SubTarea subTarea);

        Task<PagedResult<SubTarea>> ObtenerSubTareasPaginadas(int tareaId, int page, int limit, string? order);

        Task<SubTarea> ObtenerSubTareaPorId(int subTareaId);

        Task<SubTarea?> ActualizarSubTarea(SubTarea subtarea);

        Task<bool> EliminarSubTarea(SubTarea subtarea);
    }
}