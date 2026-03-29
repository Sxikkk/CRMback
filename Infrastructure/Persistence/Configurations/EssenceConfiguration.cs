using Domain.Entities;
using Domain.ValueObjects;
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
            .HasConversion<string>();

        builder.Ignore(e => e.Status);
        
        builder.Property(e => e.CreatedById)
            .IsRequired();

        builder.Property(e => e.AssignedToId);

        builder.Property(e => e.CreatedByOrganization)
            .IsRequired();

        builder.Property(e => e.CreatedForOrganization);

        builder.Property(e => e.DueDate);

        builder.Property(e => e.CreatedAtUtc)
            .IsRequired();

        builder.Property(e => e.CompletedAtUtc);

        builder.Ignore(e => e.TotalTime);

        builder.Property(e => e.EssencePrice)
            .HasConversion(
                v => v.HasValue ? v.Value.Value : (decimal?)null,
                v => v.HasValue ? new EssencePrice(v.Value) : null
            )
            .HasColumnName("Price");

        builder.OwnsMany(e => e.Stages, s =>
        {
            s.WithOwner().HasForeignKey("EssenceId");
            s.HasKey(st => st.Id);

            s.Property(st => st.Id).ValueGeneratedNever();
            s.Property(st => st.Name).IsRequired().HasMaxLength(200);
            s.Property(st => st.Order).IsRequired();
            s.Property(st => st.Status).IsRequired().HasConversion<string>();
            s.Property(st => st.StartedAt);
            s.Property(st => st.CompletedAt);
            s.Property(st => st.EstimatedDuration).IsRequired();
            s.Property(st => st.TimeSpent).IsRequired();

            s.Property("_lastStartUtc").HasColumnName("LastStartUtc");
        });

        builder.Navigation(e => e.Stages)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(e => e.CreatedById);
        builder.HasIndex(e => e.AssignedToId);
        builder.HasIndex(e => e.Priority);
        builder.HasIndex(e => e.CreatedByOrganization);
    }
}