using Core.Contracts.Request;
using Core.Contracts.Resposes;
using Core.Domain;
using Core.Domain.Entities;
using Core.Enum;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiVersion("1.0")]
    [Route("v1/api/subTask")]
    public class SubTaskController : BaseApiController
    {
        private readonly ILogger<SubTaskController> _logger;
        private readonly ISubTaskService _subTaskService;

        public SubTaskController(ILogger<SubTaskController> logger, ISubTaskService subTaskService)
        {
            _logger = logger;
            _subTaskService = subTaskService;
        }

        /// <summary>
        /// Método que inserta una Subtarea en la base de datos
        /// </summary>
        /// <param name="tareaId">Id de la tarea a la que pertenece la subtarea</param>
        /// <param name="request">Datos de la subtarea</param>
        /// <returns>Retorna el Id generado para esa subtarea</returns>
        [HttpPost("{tareaId}/subTarea")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InsertarSubTarea([FromRoute] int tareaId, [FromBody] SubTareaRequest request)
        {
            _logger.LogInformation("Iniciando inserción de subtarea para TareaId: {TareaId}", tareaId);

            try
            {
                var idGenerado = await _subTaskService.InsertarSubTarea(tareaId, request);

                _logger.LogInformation("Subtarea creada correctamente con Id {SubTareaId}", idGenerado);
                return CreatedAtAction(nameof(InsertarSubTarea), new { tareaId, id = idGenerado }, idGenerado);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la api.");
                if (ex.CodigoError == ErrorType.ErrorNoEncontrado)
                {
                    return NotFound(new ErrorResponse
                    {
                        ErrorType = ex.CodigoError,
                        ErrorMessage = ex.Message
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, CrearError(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al insertar la subtarea para TareaId: {TareaId}", tareaId);
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }

        /// <summary>
        /// Método que consulta todas las Subtareas en la base de datos
        /// </summary>
        /// <returns>Retorna todas las subtareas</returns>
        [HttpGet("{tareaId}/subTarea")]
        [ProducesResponseType(typeof(PagedResult<SubTarea>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerSubTareasPaginadas([FromRoute] int tareaId,[FromQuery] PagedRequest request)
        {
            _logger.LogInformation("Iniciando la petición para obtener subtareas de la tarea con ID: {TareaId} con paginación.", tareaId);

            try
            {
                var resultado = await _subTaskService.ObtenerSubTareasPaginadas(tareaId, request.Page, request.Limit, request.Order);

                if (resultado.Data == null || !resultado.Data.Any())
                {
                    _logger.LogWarning("No se encontraron subtareas para la tarea con ID {TareaId}.", tareaId);
                    return NoContent();
                }

                _logger.LogInformation("Se obtuvieron {Cantidad} subtareas para la tarea con ID {TareaId} en la página {Pagina}.", resultado.Data.Count, tareaId, request.Page);
                return Ok(resultado);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la api");
                if (ex.CodigoError == ErrorType.ErrorNoEncontrado)
                {
                    return NotFound(CrearError(ex));
                }
                return BadRequest(CrearError(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar obtener las subtareas para la tarea con ID {TareaId}.", tareaId);
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }

        /// <summary>
        /// Método que consulta una Subtarea específica en la base de datos
        /// </summary>
        /// <param name="tareaId">Id de la tarea a la que pertenece la subtarea</param>
        /// <param name="subTareaId">Id de la subtarea a consultar</param>
        /// <returns>Retorna una subtarea específica</returns>
        [HttpGet("{tareaId}/subTarea/{subTareaId}")]
        [ProducesResponseType(typeof(SubTarea), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerSubTareaPorId([FromRoute] int tareaId,[FromRoute] int subTareaId)
        {
            _logger.LogInformation("Iniciando la petición para obtener subtarea con ID: {SubTareaId} para TareaId: {TareaId}", subTareaId, tareaId);

            try
            {
                var subtarea = await _subTaskService.ObtenerSubTareaPorId(tareaId, subTareaId);

                _logger.LogInformation("Se encontró la subtarea con ID {SubTareaId}.", subtarea.Id);
                return Ok(subtarea);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la api.");
                if (ex.CodigoError == ErrorType.ErrorNoEncontrado)
                {
                    return NotFound(CrearError(ex));
                }
                return BadRequest(CrearError(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar obtener la subtarea con ID {SubTareaId}.", subTareaId);
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }

        /// <summary>
        /// Método que actualiza una Subtarea especifica en la base de datos
        /// </summary>
        /// <param name="tareaId">Id de la tarea padre</param>
        /// <param name="subTareaId">Id de la subtarea a actualizar</param>
        /// <param name="request">Datos de la subtarea a actualizar</param>
        /// <returns>Retorna una respuesta sin contenido si la actualización es exitosa</returns>
        [HttpPut("{tareaId}/subTarea/{subTareaId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarSubTarea([FromRoute] int tareaId,[FromRoute] int subTareaId,[FromBody] ActualizarSubTareaRequest request)
        {
            _logger.LogInformation("Iniciando actualización de subtarea con ID: {SubTareaId} para TareaId: {TareaId}", subTareaId, tareaId);

            try
            {
                await _subTaskService.ActualizarSubTarea(tareaId, subTareaId, request);

                return Ok("Subtarea actualizada correctamente.");
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la API.");
                if (ex.CodigoError == ErrorType.ErrorNoEncontrado)
                {
                    return NotFound(CrearError(ex));
                }
                return BadRequest(CrearError(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar actualizar la subtarea con ID {SubTareaId}.", subTareaId);
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }

        /// <summary>
        /// Método que elimina una Subtarea específica en la base de datos
        /// </summary>
        /// <param name="tareaId">Id de la tarea padre</param>
        /// <param name="subTareaId">Id de la subtarea a eliminar</param>
        /// <returns>Retorna un mensaje de confirmación</returns>
        [HttpDelete("{tareaId}/subTarea/{subTareaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarSubTarea([FromRoute] int tareaId,[FromRoute] int subTareaId)
        {
            _logger.LogInformation("Iniciando eliminación de subtarea con ID: {SubTareaId} para TareaId: {TareaId}", subTareaId, tareaId);

            try
            {
                await _subTaskService.EliminarSubTarea(tareaId, subTareaId);
                return Ok("Subtarea eliminada.");
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Ocurrió un error en la API.");
                if (ex.CodigoError == ErrorType.ErrorNoEncontrado)
                {
                    return NotFound(CrearError(ex));
                }
                return BadRequest(CrearError(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar eliminar la subtarea con ID {SubTareaId}.", subTareaId);
                return StatusCode(StatusCodes.Status500InternalServerError, CrearError(ex));
            }
        }
    }
}