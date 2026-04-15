using Contracts.EssenceAttachment;
using Contracts.User;
using Domain.Enums;
using Domain.ValueObjects;

namespace Contracts.Tasks;

public record EssenceDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public EEssencePriority Priority { get; set; }

    public EEssenceStatus Status { get; set; }
    public StageDto[] Stages { get; set; } = null!; 

    public Guid CreatedById { get; set; }

    public Guid? AssignedToId { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? CompletedAtUtc { get; set; }

    public TimeSpan TimeTracked { get; set; }
    public UserDto Creator { get; set; } = null!;
    public UserDto? Executor { get; set; }
    public decimal? Price { get; set; }
    public EssenceAttachmentDto[] Attachments { get; set; } = [];
}