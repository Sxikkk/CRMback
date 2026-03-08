using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EssenceConfiguration: IEntityTypeConfiguration<Essence>
{
    public void Configure(EntityTypeBuilder<Essence> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(e => e.Priority)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.CreatedById)
            .IsRequired();

        builder.Property(e => e.AssignedToId);

        builder.Property(e => e.DueDate);

        builder.Property(e => e.CreatedAtUtc)
            .IsRequired();

        builder.Property(e => e.CompletedAtUtc);

        builder.Property(e => e.TimeTracked)
            .IsRequired()
            .HasDefaultValue(TimeSpan.Zero);

        builder.HasIndex(e => e.CreatedById);
        builder.HasIndex(e => e.AssignedToId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.Priority);
    }
}