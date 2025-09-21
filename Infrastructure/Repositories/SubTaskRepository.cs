using Core.Contracts.Resposes;
using Core.Domain.Entities;
using Core.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class SubTaskRepository : ISubTaskRepository
    {
        private readonly SqlServerDbContext _context;
        private readonly ILogger<SubTaskRepository> _logger;

        public SubTaskRepository(SqlServerDbContext context, ILogger<SubTaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> InsertarSubTarea(SubTarea subTarea)
        {
            _logger.LogInformation("Insertando subtarea para tarea con ID {TareaId}.", subTarea.TareaId);

            await _context.Subtareas.AddAsync(subTarea);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Subtarea insertada correctamente con ID {SubTareaId}.", subTarea.Id);
            return subTarea.Id;
        }

        public async Task<PagedResult<SubTarea>> ObtenerSubTareasPaginadas(int tareaId, int page, int limit, string? order)
        {
            _logger.LogInformation("Buscando subtareas en la base de datos para TareaId: {TareaId}", tareaId);

            var query = _context.Subtareas.AsNoTracking().Where(st => st.TareaId == tareaId);

            if (order?.ToLower() == "desc")
            {
                query = query.OrderByDescending(st => st.FechaCreacion);
            }
            else
            {
                query = query.OrderBy(st => st.FechaCreacion);
            }

            var totalRegistros = await query.CountAsync();

            var datos = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)limit);

            _logger.LogInformation("Consulta completada. Registros: {Cantidad}, Total: {Total}", datos.Count, totalRegistros);

            return new PagedResult<SubTarea>
            {
                Data = datos,
                Meta = new Meta
                {
                    Total = totalRegistros,
                    Page = page,
                    TotalPages = totalPaginas
                }
            };
        }

        public async Task<SubTarea> ObtenerSubTareaPorId(int subTareaId)
        {
            _logger.LogInformation("Consultando subtarea con ID {SubTareaId} en la base de datos.", subTareaId);

            return await _context.Subtareas.FindAsync(subTareaId);
        }

        public async Task<SubTarea?> ActualizarSubTarea(SubTarea subtarea)
        {
            _logger.LogInformation("Actualizando subtarea con ID {SubTareaId} en la base de datos.", subtarea.Id);

            _context.Subtareas.Update(subtarea);
            var filasAfectadas = await _context.SaveChangesAsync();

            if (filasAfectadas > 0)
            {
                return subtarea;
            }
            return null;
        }

        public async Task<bool> EliminarSubTarea(SubTarea subtarea)
        {
            _logger.LogInformation("Eliminando subtarea con ID {SubTareaId} de la base de datos.", subtarea.Id);

            _context.Subtareas.Remove(subtarea);
            var filasAfectadas = await _context.SaveChangesAsync();

            return filasAfectadas > 0;
        }
    }
}