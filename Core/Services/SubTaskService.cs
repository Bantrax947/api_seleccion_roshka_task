using Core.Contracts.Request;
using Core.Contracts.Resposes;
using Core.Domain;
using Core.Domain.Entities;
using Core.Enum;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class SubTaskService : ISubTaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ISubTaskRepository _subTaskRepository;
        private readonly ILogger<SubTaskService> _logger;

        public SubTaskService(
            ITaskRepository taskRepository,
            ISubTaskRepository subTaskRepository,
            ILogger<SubTaskService> logger)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _subTaskRepository = subTaskRepository ?? throw new ArgumentNullException(nameof(subTaskRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int?> InsertarSubTarea(int tareaId, SubTareaRequest request)
        {
            _logger.LogInformation("Validando existencia de la tarea con ID {TareaId}", tareaId);

            var tareaExistente = await _taskRepository.ObtenerPorId(tareaId);
            if (tareaExistente == null)
            {
                _logger.LogWarning("Tarea con ID {TareaId} no encontrada. No se puede insertar subtarea.", tareaId);
                throw new AppException("La tarea no existe.", ErrorType.ErrorNoEncontrado);
            }

            var subTarea = new SubTarea
            {
                TareaId = tareaId,
                Titulo = request.Titulo,
                Estado = request.Estado,
                FechaCreacion = DateTime.UtcNow
            };

            var idGenerado = await _subTaskRepository.InsertarSubTarea(subTarea);

            _logger.LogInformation("Subtarea creada correctamente con ID {SubTareaId} para la TareaId {TareaId}", idGenerado, tareaId);

            return idGenerado;
        }

        public async Task<PagedResult<SubTarea>> ObtenerSubTareasPaginadas(int tareaId, int page, int limit, string? order)
        {
            _logger.LogInformation("Buscando subtareas paginadas para la tarea con ID {TareaId}", tareaId);

            var tareaExistente = await _taskRepository.ObtenerPorId(tareaId);
            if (tareaExistente == null)
            {
                _logger.LogWarning("Tarea con ID {TareaId} no encontrada. Retornando null.", tareaId);
                throw new AppException("La tarea no existe.", ErrorType.ErrorNoEncontrado);
            }

            var resultado = await _subTaskRepository.ObtenerSubTareasPaginadas(tareaId, page, limit, order);

            _logger.LogInformation("Se encontraron {Cantidad} subtareas para la tarea con ID {TareaId}.", resultado.Data.Count, tareaId);

            return resultado;
        }

        public async Task<SubTarea> ObtenerSubTareaPorId(int tareaId, int subTareaId)
        {
            _logger.LogInformation("Buscando subtarea con ID {SubTareaId} para la tarea con ID {TareaId}", subTareaId, tareaId);

            var tareaExistente = await _taskRepository.ObtenerPorId(tareaId);
            if (tareaExistente == null)
            {
                _logger.LogWarning("Tarea con ID {TareaId} no encontrada.", tareaId);
                throw new AppException("La tarea no existe.", ErrorType.ErrorNoEncontrado);
            }

            var subtarea = await _subTaskRepository.ObtenerSubTareaPorId(subTareaId);

            if (subtarea == null || subtarea.TareaId != tareaId)
            {
                _logger.LogWarning("Subtarea con ID {SubTareaId} no encontrada o no pertenece a la tarea con ID {TareaId}.", subTareaId, tareaId);
                throw new AppException("La subtarea o la tarea no existe.", ErrorType.ErrorNoEncontrado);
            }

            _logger.LogInformation("Subtarea encontrada con éxito.");
            return subtarea;
        }

        public async Task ActualizarSubTarea(int tareaId, int subTareaId, ActualizarSubTareaRequest request)
        {
            _logger.LogInformation("Verificando existencia de la tarea con ID {TareaId} y subtarea con ID {SubTareaId}.", tareaId, subTareaId);

            var tareaExistente = await _taskRepository.ObtenerPorId(tareaId);
            if (tareaExistente == null)
            {
                _logger.LogWarning("Tarea con ID {TareaId} no encontrada.", tareaId);
                throw new AppException("La tarea no existe.", ErrorType.ErrorNoEncontrado);
            }

            var subtarea = await _subTaskRepository.ObtenerSubTareaPorId(subTareaId);

            if (subtarea == null || subtarea.TareaId != tareaId)
            {
                _logger.LogWarning("Subtarea con ID {SubTareaId} no encontrada o no pertenece a la tarea con ID {TareaId}.", subTareaId, tareaId);
                throw new AppException("La subtarea o la tarea no existe.", ErrorType.ErrorNoEncontrado);
            }

            subtarea.Titulo = request.Titulo;
            subtarea.Estado = request.Estado;

            await _subTaskRepository.ActualizarSubTarea(subtarea);

            _logger.LogInformation("Subtarea con ID {SubTareaId} actualizada.", subTareaId);
        }

        public async Task EliminarSubTarea(int tareaId, int subTareaId)
        {
            _logger.LogInformation("Buscando subtarea con ID {SubTareaId} para eliminarla.", subTareaId);

            var subtarea = await _subTaskRepository.ObtenerSubTareaPorId(subTareaId);

            if (subtarea == null || subtarea.TareaId != tareaId)
            {
                _logger.LogWarning("Subtarea con ID {SubTareaId} no encontrada o no pertenece a la tarea con ID {TareaId}.", subTareaId, tareaId);
                throw new AppException("La subtarea o la tarea no existe.", ErrorType.ErrorNoEncontrado);
            }

            await _subTaskRepository.EliminarSubTarea(subtarea);
            _logger.LogInformation("Subtarea con ID {SubTareaId} eliminada correctamente.", subTareaId);
        }
    }
}