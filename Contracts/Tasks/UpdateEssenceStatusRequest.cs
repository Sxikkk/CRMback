using Domain.Enums;

namespace Contracts.Tasks;

public record UpdateEssenceStatusRequest
{
    public EEssencePriority Priority { get; init; }
};