using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Essence
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public EEssencePriority Priority { get; private set; }

    public EEssenceStatus Status => CalculateOverallStatus();

    public Guid CreatedById { get; private set; }
    public Guid? AssignedToId { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime CreatedAtUtc { get; private init; }
    public DateTime? CompletedAtUtc { get; private set; }

    public Guid CreatedByOrganization { get; private set; }
    public Guid? CreatedForOrganization { get; private set; }

    public EssencePrice? EssencePrice { get; private set; }
    public IReadOnlyCollection<EssenceStage> Stages => _stages.AsReadOnly();

    private readonly List<EssenceStage> _stages = new();

    private Essence() { }

    public static Essence Create(string title, Guid createdById, Guid organizationId, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty");

        return new Essence
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Description = description?.Trim(),
            Priority = EEssencePriority.Normal,
            CreatedById = createdById,
            CreatedAtUtc = DateTime.UtcNow,
            CreatedByOrganization = organizationId,
        };
    }

    public void AssignTo(Guid? userId)
    {
        AssignedToId = userId;
    }
    
    public void AssignToOrganization(Guid? organizationId)
    {
        CreatedForOrganization = organizationId;
    }

    public void SetDueDate(DateTime? dueDate)
    {
        if (dueDate.HasValue && dueDate.Value < CreatedAtUtc)
            throw new DomainException("Due date cannot be before creation date");

        DueDate = dueDate?.ToUniversalTime();
    }

    public void ChangePriority(EEssencePriority priority)
    {
        Priority = priority;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty");

        Title = title.Trim();
    }

    public void UpdateDescription(string? description)
    {
        Description = description?.Trim();
    }

    public TimeSpan TotalTime =>
        TimeSpan.FromTicks(_stages.Sum(s => s.GetCurrentTimeSpent().Ticks));

    public void AddStage(string name, int order, TimeSpan? estimatedDuration = null)
    {
        if (_stages.Any(s => s.Order == order))
            throw new DomainException("Stage with this order already exists");

        var stage = EssenceStage.Create(Id, name, order, estimatedDuration);
        _stages.Add(stage);
    }

    public void StartStage(Guid stageId)
    {
        var stage = GetStageOrThrow(stageId);

        var previousStages = _stages.Where(s => s.Order < stage.Order);
        if (previousStages.Any(s => s.Status != EEssenceStatus.Completed))
            throw new DomainException("Previous stages must be completed");

        stage.Start();
    }

    public void PauseStage(Guid stageId)
    {
        var stage = GetStageOrThrow(stageId);
        stage.Pause();
    }

    public void CompleteStage(Guid stageId)
    {
        var stage = GetStageOrThrow(stageId);
        stage.Complete();

        if (_stages.All(s => s.Status == EEssenceStatus.Completed))
            CompletedAtUtc = DateTime.UtcNow;
    }

    public void SetPrice(EssencePrice? price)
    {
        EssencePrice = price;
    }

    public void ReopenStage(Guid stageId)
    {
        var stage = GetStageOrThrow(stageId);
        stage.Reopen();

        CompletedAtUtc = null;
    }

    public void ReorderStages(IReadOnlyList<(Guid StageId, int NewOrder)>? changes)
    {
        if (changes == null || changes.Count == 0)
            return;

        var stageIds = changes.Select(c => c.StageId).ToHashSet();
        if (stageIds.Count != changes.Count)
            throw new DomainException("Duplicate stage IDs");

        var newOrders = changes.Select(c => c.NewOrder).ToList();

        if (newOrders.Any(o => o < 0))
            throw new DomainException("Order cannot be negative");

        var expected = Enumerable.Range(0, changes.Count);
        if (!expected.OrderBy(x => x).SequenceEqual(newOrders.OrderBy(x => x)))
            throw new DomainException("Orders must be continuous starting from 0");

        var dict = _stages.ToDictionary(s => s.Id);

        foreach (var (stageId, newOrder) in changes)
        {
            if (!dict.TryGetValue(stageId, out var value))
                throw new DomainException($"Stage {stageId} not found");
            value.ChangeOrder(newOrder);
        }

        _stages.Sort((a, b) => a.Order.CompareTo(b.Order));
    }

    private EssenceStage GetStageOrThrow(Guid stageId)
    {
        var stage = _stages.FirstOrDefault(s => s.Id == stageId);
        if (stage == null)
            throw new DomainException("Stage not found");

        return stage;
    }

    private EEssenceStatus CalculateOverallStatus()
    {
        if (_stages.Count == 0)
            return EEssenceStatus.Waiting;

        if (_stages.All(s => s.Status == EEssenceStatus.Completed))
            return EEssenceStatus.Completed;

        if (_stages.Any(s => s.Status == EEssenceStatus.InProgress))
            return EEssenceStatus.InProgress;

        if (_stages.Any(s => s.Status == EEssenceStatus.Paused))
            return EEssenceStatus.Paused;

        return EEssenceStatus.Waiting;
    }
}