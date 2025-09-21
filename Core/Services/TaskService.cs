using Core.Contracts.Request;
using Core.Contracts.Resposes;
using Core.Domain;
using Core.Domain.Entities;
using Core.Enum;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<int> InsertarTarea(TareaRequest request)
        {
            _logger.LogInformation("Insertando una nueva tarea en la base de datos.");

            var tarea = new Tarea
            {
                Titulo = request.Titulo,
                Descripcion = request.Descripcion,
                FechaCreacion = DateTime.Now,
                FechaVencimiento = request.FechaVencimiento,
                Estado = request.Estado,
                Prioridad = request.Prioridad
            };

            var tareaId = await _taskRepository.InsertarTarea(tarea);

            _logger.LogInformation("Tarea insertada con éxito. ID: {TareaId}", tareaId);

            return tareaId;
        }

        public async Task<PagedResult<Tarea>> ObtenerTareasPaginadas(PagedRequest request)
        {
            _logger.LogInformation("Servicio: obteniendo tareas. Página: {Pagina}, Tamaño: {Cantidad}", request.Page, request.Limit);

            return await _taskRepository.ObtenerTareasPaginadas(request.Page, request.Limit, request.Order);
        }

        public async Task<Tarea> ObtenerTareaId(int id)
        {
            _logger.LogInformation("Llamando al repositorio para obtener tarea con Id {Id}.", id);

            var tarea = await _taskRepository.ObtenerTareaId(id);

            if (tarea == null)
            {
                _logger.LogWarning("Servicio: No se encontró la tarea con ID {Id}.", id);
                throw new AppException("La tarea no existe.", ErrorType.ErrorNoEncontrado);
            }

            _logger.LogInformation("Servicio: Tarea con ID {Id} obtenida exitosamente.", id);
            return tarea;
        }

        public async Task ActualizarTarea(int id, ActualizarTareaRequest request)
        {
            _logger.LogInformation("Servicio: Validando y actualizando tarea con ID {TareaId}", id);

            var tareaExistente = await _taskRepository.ObtenerPorId(id);

            if (tareaExistente == null)
            {
                _logger.LogWarning("Servicio: Tarea con ID {TareaId} no encontrada.", id);
                throw new AppException("La tarea no existe.", ErrorType.ErrorNoEncontrado);
            }

            var existeTitulo = await _taskRepository.ExisteTituloEnOtraTarea(request.Titulo, id);
            if (existeTitulo)
            {
                _logger.LogWarning("Servicio: El título '{Titulo}' ya existe en otra tarea.", request.Titulo);
                throw new AppException("Ya existe otra tarea con el mismo título.", ErrorType.ErrorValidacion);
            }

            tareaExistente.Titulo = request.Titulo;
            tareaExistente.Descripcion = request.Descripcion;
            tareaExistente.FechaVencimiento = request.FechaVencimiento ?? DateTime.Now;
            tareaExistente.Estado = request.Estado;
            tareaExistente.Prioridad = request.Prioridad;

            await _taskRepository.ActualizarTarea(tareaExistente);

            _logger.LogInformation("Servicio: Tarea con ID {TareaId} actualizada correctamente.", tareaExistente.Id);
        }

        public async Task EliminarTarea(int id)
        {
            _logger.LogInformation("Servicio: Verificando la existencia de la tarea con ID {TareaId}", id);

            var tareaExistente = await _taskRepository.ObtenerPorId(id);
            if (tareaExistente == null)
            {
                _logger.LogWarning("Servicio: Tarea con ID {TareaId} no encontrada.", id);
                // La lógica de negocio determina que el recurso no existe y lanza una excepción.
                throw new AppException("La tarea no existe.", Core.Enum.ErrorType.ErrorNoEncontrado);
            }

            await _taskRepository.EliminarTarea(id);

            _logger.LogInformation("Servicio: Tarea con ID {TareaId} eliminada correctamente.", id);
        }
    }
}
