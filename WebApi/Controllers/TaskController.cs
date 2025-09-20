using Core.Contracts.Resposes;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v1/api/task")]
    public class TaskController : BaseApiController
    {

        /// <summary>
        /// Método que inserta una tarea eb la base de datos
        /// </summary>
        /// <returns>Retorna el Id generado para esa tarea</returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InsertarTarea()
        {
            return Created("", null);
        }

        /// <summary>
        /// Método que lista todas las tareas de la base de datos
        /// </summary>
        /// <returns>Retorna el Id generado para esa tarea</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerTarea()
        {
            return Created("", null);
        }

        /// <summary>
        /// Método que lista todas las tareas de la base de datos
        /// </summary>
        /// <returns>Retorna el Id generado para esa tarea</returns>
        [HttpGet("id")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerTareaId()
        {
            return Created("", null);
        }

        /// <summary>
        /// Método que lista todas las tareas de la base de datos
        /// </summary>
        /// <returns>Retorna el Id generado para esa tarea</returns>
        [HttpPut("id")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarTarea()
        {
            return Created("", null);
        }

        /// <summary>
        /// Método que lista todas las tareas de la base de datos
        /// </summary>
        /// <returns>Retorna el Id generado para esa tarea</returns>
        [HttpDelete("id")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarTarea()
        {
            return Created("", null);
        }

        /// <summary>
        /// Método que inserta una Subtarea en la base de datos
        /// </summary>
        /// <returns>Retorna el Id generado para esa tarea</returns>
        [HttpPost("id/subTarea")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InsertarSubTarea()
        {
            return Created("", null);
        }

        /// <summary>
        /// Método que consulta todas las Subtareas en la base de datos
        /// </summary>
        /// <returns>Retorna todas las subtareas</returns>
        [HttpGet("id/subTarea")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerSubTarea()
        {
            return Created("", null);
        }

        /// <summary>
        /// Método que consulta una Subtarea especifica en la base de datos
        /// </summary>
        /// <returns>Retorna una subtarea espeficica</returns>
        [HttpGet("id/subTarea/id")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerSubTareaId()
        {
            return Created("", null);
        }


        /// <summary>
        /// Método que actualiza una Subtarea especifica en la base de datos
        /// </summary>
        /// <returns>Retorna una subtarea espeficica actualizada</returns>
        [HttpPut("id/subTarea/id")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarSubTareaId()
        {
            return Created("", null);
        }

        /// <summary>
        /// Método que elimina una Subtarea especifica en la base de datos
        /// </summary>
        /// <returns>Retorna un mensaje de confirmación</returns>
        [HttpDelete("id/subTarea/id")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarrSubTareaId()
        {
            return Created("", null);
        }
    }
}
