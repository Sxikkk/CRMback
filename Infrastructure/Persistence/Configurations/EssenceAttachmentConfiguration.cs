using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EssenceAttachmentConfiguration: IEntityTypeConfiguration<EssenceAttachment>
{
    public void Configure(EntityTypeBuilder<EssenceAttachment> builder)
    {
        builder.ToTable("EssenceAttachments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.FileName).IsRequired().HasMaxLength(500);
        builder.Property(a => a.FilePath).IsRequired().HasMaxLength(1000);
        builder.Property(a => a.ContentType).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Size).IsRequired();

        builder.Property(a => a.UploadedAtUtc).IsRequired();

        builder.HasOne<Essence>()
            .WithMany(e => e.Attachments)
            .HasForeignKey(a => a.EssenceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(a => a.EssenceId);
        builder.HasIndex(a => a.Id).IsUnique();
        builder.HasIndex(a => a.FilePath).IsUnique();
    }
}