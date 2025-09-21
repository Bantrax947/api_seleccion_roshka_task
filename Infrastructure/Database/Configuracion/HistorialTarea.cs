using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Domain.Entities;

namespace Infrastructure.Database.Configurations
{
    public class HistorialTareaConfiguration : IEntityTypeConfiguration<HistorialTarea>
    {
        public void Configure(EntityTypeBuilder<HistorialTarea> builder)
        {
            builder.ToTable("Historial_Tareas");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd().IsRequired();
            builder.Property(e => e.TareaId).HasColumnName("TareaId").IsRequired();
            builder.Property(e => e.EstadoAnterior).HasColumnName("EstadoAnterior").HasMaxLength(50);
            builder.Property(e => e.EstadoNuevo).HasColumnName("EstadoNuevo").HasMaxLength(50).IsRequired();
            builder.Property(e => e.FechaCambio).HasColumnName("FechaCambio").IsRequired();

            builder.HasOne<Tarea>()
                .WithMany()
                .HasForeignKey(e => e.TareaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}