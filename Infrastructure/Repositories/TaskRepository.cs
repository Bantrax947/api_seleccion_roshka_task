using Core.Contracts.Resposes;
using Core.Domain.Entities;
using Core.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly SqlServerDbContext _context;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(SqlServerDbContext context, ILogger<TaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> InsertarTarea(Tarea tarea)
        {
            _logger.LogInformation("Insertando una nueva tarea en la base de datos.");

            await _context.Tareas.AddAsync(tarea);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea insertada con éxito. ID: {TareaId}", tarea.Id);

            return tarea.Id;
        }

        public async Task<PagedResult<Tarea>> ObtenerTarea(int pagina, int cantidadPorPagina)
        {
            _logger.LogInformation("Consultando tareas con paginación. Página {Pagina}, Tamaño {Cantidad}", pagina, cantidadPorPagina);

            var query = _context.Tareas.AsNoTracking();

            var totalRegistros = await query.CountAsync();

            var datos = await query
                .OrderByDescending(t => t.FechaCreacion)
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)cantidadPorPagina);

            _logger.LogInformation("Consulta completada. Registros: {Cantidad}, Total: {Total}", datos.Count, totalRegistros);

            return new PagedResult<Tarea>
            {
                Data = datos,
                Meta = new Meta
                {
                    Total = totalRegistros,
                    Page = pagina,
                    TotalPages = totalPaginas
                }
            };
        }

        public async Task<Tarea?> ObtenerTareaId(int id)
        {
            _logger.LogInformation("Buscando tarea con Id {Id} en la base de datos.", id);

            var tarea = await _context.Tareas
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            return tarea;
        }

        public async Task<bool> ActualizarTarea(Tarea tarea)
        {
            _logger.LogInformation("Actualizando tarea con ID {TareaId} en la base de datos.", tarea.Id);

            var existingTask = await _context.Tareas.FindAsync(tarea.Id);
            if (existingTask == null)
                return false;

            existingTask.Titulo = tarea.Titulo;
            existingTask.Descripcion = tarea.Descripcion;
            existingTask.FechaVencimiento = tarea.FechaVencimiento;
            existingTask.Estado = tarea.Estado;
            existingTask.Prioridad = tarea.Prioridad;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea con ID {TareaId} actualizada correctamente.", tarea.Id);
            return true;
        }
        public async Task<Tarea> ObtenerPorId(int id)
        {
            return await _context.Tareas.FindAsync(id);
        }

        public async Task<bool> ExisteTituloEnOtraTarea(string titulo, int id)
        {
            return await _context.Tareas.AnyAsync(t => t.Titulo == titulo && t.Id != id);
        }

        public async Task<bool> EliminarTarea(int id)
        {
            _logger.LogInformation("Eliminando tarea con ID {TareaId} en la base de datos.", id);

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                _logger.LogInformation("Tarea con ID {TareaId} no encontrada.", id);
                return false; 
            }

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Tarea con ID {TareaId} eliminada correctamente.", id);
            return true;
        }
    }
}