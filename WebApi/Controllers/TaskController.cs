using Core.Contracts.Request;
using Core.Contracts.Resposes;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Domain;

namespace WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v1/api/task")]
    public class TaskController : BaseApiController
    {

        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        /// <summary>
        /// Método que inserta una tarea en la base de datos
        /// </summary>
        /// <param name="request">Datos para la nueva tarea</param>
        /// <returns>Retorna el Id generado para esa tarea</returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InsertarTarea([FromBody] TareaRequest request)
        {
            _logger.LogInformation("Inicio de la inserción de una nueva tarea.");

            try
            {
                var tarea = new Tarea
                {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaCreacion = DateTime.Now,
                    FechaVencimiento = request.FechaVencimiento,
                    Estado = request.Estado,
                    Prioridad = request.Prioridad
                };

                var tareaId = await _taskService.InsertarTarea(tarea);
                   
                return StatusCode(StatusCodes.Status201Created, tareaId);
            }
            catch(AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrio un error en la api");
                return StatusCode(StatusCodes.Status400BadRequest, CrearError(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar insertar la tarea.");
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }

        /// <summary>
        /// Método que lista todas las tareas de la base de datos con paginación
        /// </summary>
        /// <param name="pagina">Número de página (por defecto 1)</param>
        /// <param name="cantidadPorPagina">Cantidad de registros por página (por defecto 10)</param>
        /// <returns>Retorna un PagedResult con las tareas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<Tarea>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerTarea([FromQuery] int pagina = 1, [FromQuery] int cantidadPorPagina = 5)
        {
            _logger.LogInformation("Iniciando la petición para obtener tareas. Página: {Pagina}, Tamaño: {Cantidad}", pagina, cantidadPorPagina);

            try
            {
                var resultado = await _taskService.ObtenerTarea(pagina, cantidadPorPagina);

                if (resultado.Data == null || !resultado.Data.Any())
                {
                    _logger.LogWarning("No se encontraron tareas en la base de datos para la página {Pagina}.", pagina);
                    return NoContent();
                }

                _logger.LogInformation("Se obtuvieron {Cantidad} tareas para la página {Pagina}.", resultado.Data.Count, pagina);
                return Ok(resultado);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrio un error en la api");
                return StatusCode(StatusCodes.Status400BadRequest, CrearError(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar obtener las tareas.");
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }



        /// <summary>
        /// Método que obtiene una tarea específica por su Id.
        /// </summary>
        /// <param name="id">Id de la tarea</param>
        /// <returns>Retorna la tarea encontrada o 404 si no existe</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Tarea), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerTareaId([FromRoute] int id)
        {
            _logger.LogInformation("Iniciando la petición para obtener tarea con Id: {Id}", id);

            try
            {
                var tarea = await _taskService.ObtenerTareaId(id);

                if (tarea == null)
                {
                    _logger.LogWarning("No se encontró ninguna tarea con Id {Id}.", id);
                    return NoContent();
                }

                _logger.LogInformation("Tarea con Id {Id} obtenida exitosamente.", id);
                return Ok(tarea);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la API al obtener tarea con Id {Id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, CrearError(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar obtener la tarea con Id {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }


        /// <summary>
        /// Método que actualiza una tarea existente
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarTarea(int id, [FromBody] ActualizarTareaRequest request)
        {
            _logger.LogInformation("Inicio de la actualización de tarea con ID {TareaId}", id);

            try
            {
                var tarea = new Tarea
                {
                    Id = id,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaVencimiento = request.FechaVencimiento ?? DateTime.Now,
                    Estado = request.Estado,
                    Prioridad = request.Prioridad
                };

                var actualizado = await _taskService.ActualizarTarea(tarea);

                if (!actualizado)
                    return NoContent();

                return Ok("Tarea actualizada correctamente."); 
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Error de validación en la API");
                return BadRequest(CrearError(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar actualizar la tarea.");
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }


        /// <summary>
        /// Método que lista todas las tareas de la base de datos
        /// </summary>
        /// <returns>Retorna el Id generado para esa tarea</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            _logger.LogInformation("Inicio eliminación de tarea con ID {TareaId}", id);

            try
            {
                var eliminado = await _taskService.EliminarTarea(id);

                if (!eliminado)
                    return NoContent(); 

                return Ok("Tarea eliminada");
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la API");
                return BadRequest(CrearError(ex)); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar tarea");
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }       
    }
}
