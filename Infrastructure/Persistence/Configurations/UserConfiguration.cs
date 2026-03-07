using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        var emailConverter = new ValueConverter<Email, string>(
            v => v.Value,
            v => Email.Create(v)
        );

        var phoneConverter = new ValueConverter<Phone, string>(
            v => v.Value,
            v => Phone.Create(v)
        );

        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Id).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.UserName).IsUnique();
        
        builder.Property(u => u.Email)
            .HasConversion(emailConverter)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(u => u.Phone)
            .HasConversion(phoneConverter)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.UserName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Surname)
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .HasMany(u => u.RefreshTokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
}