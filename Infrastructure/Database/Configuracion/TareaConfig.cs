using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Domain.Entities;

namespace Infrastructure.Database.Configurations
{
    public class TareaConfiguration : IEntityTypeConfiguration<Tarea>
    {
        public void Configure(EntityTypeBuilder<Tarea> builder)
        {
            builder.ToTable("Tareas");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasDefaultValueSql("NEWSEQUENTIALID()").IsRequired();
            builder.Property(e => e.Titulo).HasColumnName("Titulo").HasMaxLength(255).IsRequired();
            builder.Property(e => e.Descripcion).HasColumnName("Descripcion");
            builder.Property(e => e.FechaCreacion).HasColumnName("FechaCreacion").IsRequired();
            builder.Property(e => e.FechaVencimiento).HasColumnName("FechaVencimiento");
            builder.Property(e => e.Estado).HasColumnName("Estado").HasMaxLength(50).IsRequired();
            builder.Property(e => e.Prioridad).HasColumnName("Prioridad").IsRequired();
        }
    }
}