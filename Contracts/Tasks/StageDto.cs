using Domain.Enums;

namespace Contracts.Tasks;

public sealed record StageDto(
    Guid Id,
    Guid EssenceId,
    string Name,
    int Order,
    EEssenceStatus Status,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    TimeSpan EstimatedDuration,
    TimeSpan TimeSpent
);