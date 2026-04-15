namespace Domain.Entities;

public class EssenceAttachment
{
    public Guid Id { get; private set; }
    public Guid EssenceId { get; private set; }
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public long Size { get; private set; }
    public string ContentType { get; private set; }
    public Guid UploadedById { get; private set; }
    public DateTime UploadedAtUtc { get; private init; }

    private EssenceAttachment() { }

    public static EssenceAttachment Create(Guid essenceId, string fileName, string filePath, 
        long size, string contentType, Guid uploadedById)
    {
        return new EssenceAttachment
        {
            Id = Guid.NewGuid(),
            EssenceId = essenceId,
            FileName = fileName,
            FilePath = filePath,
            Size = size,
            ContentType = contentType,
            UploadedById = uploadedById,
            UploadedAtUtc = DateTime.UtcNow
        };
    }
}