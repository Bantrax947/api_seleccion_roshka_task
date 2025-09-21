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

        public async Task<int> InsertarTarea(Tarea tarea)
        {
            _logger.LogInformation("Llamando al repositorio para insertar una nueva tarea.");
            return await _taskRepository.InsertarTarea(tarea);
        }

        public async Task<PagedResult<Tarea>> ObtenerTarea(int pagina, int cantidadPorPagina)
        {
            _logger.LogInformation("Servicio: obteniendo tareas. Página: {Pagina}, Tamaño: {Cantidad}", pagina, cantidadPorPagina);

            return await _taskRepository.ObtenerTarea(pagina, cantidadPorPagina);
        }

        public async Task<Tarea?> ObtenerTareaId(int id)
        {
            _logger.LogInformation("Llamando al repositorio para obtener tarea con Id {Id}.", id);

            var tarea = await _taskRepository.ObtenerTareaId(id);

            return tarea;
        }

        public async Task<bool> ActualizarTarea(Tarea tarea)
        {
            _logger.LogInformation("Validando negocio para actualizar tarea con ID {TareaId}", tarea.Id);

            var tareaExistente = await _taskRepository.ObtenerPorId(tarea.Id);
            if (tareaExistente == null)
                return false; 

            var existeTitulo = await _taskRepository.ExisteTituloEnOtraTarea(tarea.Titulo, tarea.Id);
            if (existeTitulo)
                throw new AppException("Ya existe otra tarea con el mismo título.", ErrorType.ErrorValidacion);

            _logger.LogInformation("Actualizando tarea con ID {TareaId}", tarea.Id);
            return await _taskRepository.ActualizarTarea(tarea);
        }

        public async Task<bool> EliminarTarea(int id)
        {
            _logger.LogInformation("Llamando al repositorio para eliminar tarea con ID {TareaId}", id);

            var tareaExistente = await _taskRepository.ObtenerPorId(id);
            if (tareaExistente == null)
                return false;

            return await _taskRepository.EliminarTarea(id);
        }
    }
}
