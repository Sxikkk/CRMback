using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EssenceRepository: IEssenceRepository
{
    private readonly AppDbContext _context;

    public EssenceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Essence?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Essences.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Essence>> GetByCreatorAsync(Guid creatorId, CancellationToken cancellationToken = default)
    {
        return await _context.Essences.Where(e => e.CreatedById == creatorId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Essence>> GetByAssignedUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Essences.Where(e => e.AssignedToId == userId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Essence>> GetByStatusAsync(EEssenceStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Essences.Where(e => e.Status == status).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Essence>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Essences.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Essence essence, CancellationToken cancellationToken = default)
    {
        await _context.Essences.AddAsync(essence, cancellationToken);
    }

    public void Update(Essence essence)
    {
        _context.Essences.Update(essence);        
    }

    public void Remove(Essence essence)
    {
        _context.Essences.Remove(essence);
    }

    public async Task<IReadOnlyList<Essence>> GetUserTasksAsync(Guid userId, EEssenceStatus? status, EEssencePriority? priority,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Essences
            .Where(e => e.AssignedToId == userId)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(e => e.Priority == priority.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}