using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class EssenceStage
{
    public Guid Id { get; private set; }
    public Guid EssenceId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Order { get; private set; }
    public EEssenceStatus Status { get; private set; }

    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public Guid? ResponsibleId { get; private set; }
    public TimeSpan EstimatedDuration { get; private set; } = TimeSpan.Zero;
    public TimeSpan TimeSpent { get; private set; } = TimeSpan.Zero;

    private DateTime? _lastStartUtc;

    private EssenceStage() { }

    public static EssenceStage Create(
        Guid essenceId,
        string name,
        int order,
        Guid? responsibleId = null,
        TimeSpan? estimatedDuration = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Stage name cannot be empty");

        if (order < 0)
            throw new DomainException("Order cannot be negative");

        return new EssenceStage
        {
            Id = Guid.NewGuid(),
            EssenceId = essenceId,
            Name = name.Trim(),
            Order = order,
            Status = EEssenceStatus.Waiting,
            ResponsibleId = responsibleId,
            EstimatedDuration = estimatedDuration ?? TimeSpan.Zero
        };
    }

    internal void Start()
    {
        if (Status == EEssenceStatus.InProgress) return;

        if (Status != EEssenceStatus.Waiting && Status != EEssenceStatus.Paused)
            throw new DomainException("Can start only from Waiting or Paused");

        Status = EEssenceStatus.InProgress;
        StartedAt ??= DateTime.UtcNow;
        _lastStartUtc = DateTime.UtcNow;
    }

    internal void Pause()
    {
        if (Status != EEssenceStatus.InProgress) return;

        if (_lastStartUtc.HasValue)
        {
            TimeSpent += DateTime.UtcNow - _lastStartUtc.Value;
            _lastStartUtc = null;
        }

        Status = EEssenceStatus.Paused;
    }

    internal void Complete()
    {
        if (Status == EEssenceStatus.Completed) return;

        if (Status == EEssenceStatus.InProgress)
            Pause();

        Status = EEssenceStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    internal void Reopen()
    {
        if (Status != EEssenceStatus.Completed) return;

        Status = EEssenceStatus.Paused;
        CompletedAt = null;
    }

    internal void ChangeOrder(int newOrder)
    {
        if (newOrder < 0)
            throw new DomainException("Order cannot be negative");

        Order = newOrder;
    }

    public TimeSpan GetCurrentTimeSpent()
    {
        var total = TimeSpent;

        if (Status == EEssenceStatus.InProgress && _lastStartUtc.HasValue)
            total += DateTime.UtcNow - _lastStartUtc.Value;

        return total;
    }
}