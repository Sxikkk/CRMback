using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Essence
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public EEssencePriority Priority { get; private set; }
    public EEssenceStatus Status { get; private set; }
    public Guid CreatedById { get; private set; }
    public Guid? AssignedToId { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime CreatedAtUtc { get; private init; }
    public DateTime? CompletedAtUtc { get; private set; }
    public TimeSpan TimeTracked { get; private set; } = TimeSpan.Zero;

    private DateTime? _lastStartUtc;

    private Essence() { }

    public static Essence Create(string title, Guid createdById, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty");

        return new Essence
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Description = description?.Trim(),
            Priority = EEssencePriority.Normal,
            Status = EEssenceStatus.Waiting,
            CreatedById = createdById,
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    public void AssignTo(Guid? userId)
    {
        AssignedToId = userId;
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
    
    public void ChangeStatus(EEssenceStatus status)
    {
        Status = status;
    }

    public void Start()
    {
        if (Status == EEssenceStatus.InProgress)
            return;
        
        if (Status != EEssenceStatus.Waiting && Status != EEssenceStatus.Paused)
            throw new DomainException("Можно начинать только из Waiting или Paused");

        Status = EEssenceStatus.InProgress;
        _lastStartUtc = DateTime.UtcNow;
        
    }

    public void Pause()
    {
        if (Status != EEssenceStatus.InProgress)
            return;

        if (_lastStartUtc.HasValue)
        {
            TimeTracked += DateTime.UtcNow - _lastStartUtc.Value;
            _lastStartUtc = null;
        }

        Status = EEssenceStatus.Paused;
    }

    public void Complete()
    {
        if (Status == EEssenceStatus.Completed)
            return;

        if (Status == EEssenceStatus.InProgress)
            Pause();

        Status = EEssenceStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
    }

    public void Reopen()
    {
        if (Status != EEssenceStatus.Completed)
            return;

        Status = EEssenceStatus.Paused;
        CompletedAtUtc = null;
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
    
    public TimeSpan GetCurrentTrackedTime()
    {
        var total = TimeTracked;
        if (Status == EEssenceStatus.InProgress && _lastStartUtc.HasValue)
        {
            total += DateTime.UtcNow - _lastStartUtc.Value;
        }
        return total;
    }
}