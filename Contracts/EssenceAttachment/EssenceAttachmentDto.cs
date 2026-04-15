using Contracts.User;

namespace Contracts.EssenceAttachment;

public record EssenceAttachmentDto
{
    public Guid Id { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string FilePath { get; init; } = string.Empty;
    public long Size { get; init; }
    public string ContentType { get; init; } = string.Empty;
    public DateTime UploadedAtUtc { get; init; }
    public UserDto? UploadedBy { get; init; }
}