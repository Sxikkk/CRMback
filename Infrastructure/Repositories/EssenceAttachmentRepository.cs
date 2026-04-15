using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EssenceAttachmentRepository: IEssenceAttachmentRepository
{
    private readonly AppDbContext _context;

    public EssenceAttachmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EssenceAttachment?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.EssenceAttachments.FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task<IEnumerable<EssenceAttachment>> GetByEssenceIdAsync(Guid essenceId, CancellationToken ct = default)
    {
        return await _context.EssenceAttachments.Where(f => f.EssenceId == essenceId).ToListAsync(ct);
    }

    public async Task AddAsync(EssenceAttachment attachment, CancellationToken ct = default)
    {
        await _context.EssenceAttachments.AddAsync(attachment, ct);   
    }

    public Task DeleteAsync(EssenceAttachment attachment, CancellationToken ct = default)
    {
        _context.EssenceAttachments.Remove(attachment);
        return Task.CompletedTask;
    }
}