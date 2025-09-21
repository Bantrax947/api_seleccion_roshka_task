using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Domain.Entities;

namespace Infrastructure.Database.Configurations
{
    public class SubtareaConfiguration : IEntityTypeConfiguration<SubTarea>
    {
        public void Configure(EntityTypeBuilder<SubTarea> builder)
        {
            builder.ToTable("Subtareas");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("Id").HasDefaultValueSql("NEWSEQUENTIALID()").IsRequired();
            builder.Property(e => e.TareaId).HasColumnName("TareaId").IsRequired();
            builder.Property(e => e.Titulo).HasColumnName("Titulo").IsRequired();
            builder.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
            builder.Property(e => e.FechaCreacion).HasColumnName("FechaCreacion").IsRequired();

            builder.HasOne<Tarea>()
                .WithMany()
                .HasForeignKey(e => e.TareaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}