using Core.Contracts.Request;
using Core.Contracts.Resposes;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Domain;
using Core.Enum;

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
                var tareaId = await _taskService.InsertarTarea(request);

                return CreatedAtAction(nameof(InsertarTarea), new { id = tareaId }, tareaId);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la api");
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
        /// <returns>Retorna un PagedResult con las tareas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<Tarea>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerTarea([FromQuery] PagedRequest request)
        {
            _logger.LogInformation("Iniciando la petición para obtener tareas. Página: {Pagina}, Cantidad: {Cantidad}", request.Page, request.Limit);

            try
            {
                var resultado = await _taskService.ObtenerTareasPaginadas(request);

                if (resultado.Data == null || !resultado.Data.Any())
                {
                    _logger.LogInformation("No se encontraron tareas, devolviendo una lista vacía.");
                    resultado.Data = new List<Tarea>();
                }

                _logger.LogInformation("Se obtuvieron {Cantidad} tareas para la página {Pagina}.", resultado.Data.Count, request.Page);
                return Ok(resultado);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la api");

                if (ex.CodigoError == ErrorType.ErrorNoEncontrado)
                {
                    return NotFound(new ErrorResponse { ErrorMessage = ex.Message });
                }
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
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerTareaId([FromRoute] int id)
        {
            _logger.LogInformation("Iniciando la petición para obtener tarea con Id: {Id}", id);

            try
            {
                var tarea = await _taskService.ObtenerTareaId(id);

                _logger.LogInformation("Tarea con Id {Id} obtenida exitosamente.", id);
                return Ok(tarea);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la API al obtener tarea con Id {Id}", id);

                if (ex.CodigoError == ErrorType.ErrorNoEncontrado)
                {
                    return NotFound(CrearError(ex));
                }

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
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarTarea(int id, [FromBody] ActualizarTareaRequest request)
        {
            _logger.LogInformation("Inicio de la actualización de tarea con ID {TareaId}", id);

            try
            {
                await _taskService.ActualizarTarea(id, request);

                _logger.LogInformation("Tarea actualizada correctamente.");
                return Ok("Tarea actualizada correctamente.");
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Error de validación en la API");
                if (ex.CodigoError == ErrorType.ErrorNoEncontrado)
                {
                    return NotFound(CrearError(ex));
                }
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
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            _logger.LogInformation("Inicio eliminación de tarea con ID {TareaId}", id);

            try
            {
                await _taskService.EliminarTarea(id);

                return Ok("Tarea eliminada");
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la API");
                if (ex.CodigoError == Core.Enum.ErrorType.ErrorNoEncontrado)
                {
                    return NotFound(CrearError(ex));
                }
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