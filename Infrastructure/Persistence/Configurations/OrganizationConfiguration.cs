using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever();

        builder.Property(o => o.Name)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(o => o.Inn)
            .HasMaxLength(12);

        builder.Property(o => o.Ogrn)
            .HasMaxLength(13);

        builder.Property(o => o.Email)
            .HasMaxLength(256);

        builder.Property(o => o.Phone)
            .HasMaxLength(20);

        builder.Property(o => o.Website)
            .HasMaxLength(200);

        builder.Property(o => o.City)
            .HasMaxLength(100);

        builder.Property(o => o.CreateDate)
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();

        builder.Property(o => o.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue(EOrganizationType.Company);

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue(EOrganizationStatus.Active);

        builder.HasIndex(o => o.Inn)
            .IsUnique()
            .HasFilter("\"Inn\" IS NOT NULL");

        builder.HasIndex(o => o.Name);
    }
}