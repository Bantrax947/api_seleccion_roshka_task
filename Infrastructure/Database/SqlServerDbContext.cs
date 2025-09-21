using Microsoft.EntityFrameworkCore;
using Core.Domain.Entities;
using System.Reflection;

namespace Infrastructure.Database
{
    public class SqlServerDbContext : DbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options)
        {
        }

        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<SubTarea> Subtareas { get; set; }
        public DbSet<HistorialTarea> HistorialTareas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}