using Contracts.User;
using Domain.Enums;

namespace Contracts.Tasks;

public class EssenceDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public EEssencePriority Priority { get; set; }

    public EEssenceStatus Status { get; set; }

    public Guid CreatedById { get; set; }

    public Guid? AssignedToId { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? CompletedAtUtc { get; set; }

    public TimeSpan TimeTracked { get; set; }
    public UserDto Creator { get; set; }
    public UserDto? Executor { get; set; }
}