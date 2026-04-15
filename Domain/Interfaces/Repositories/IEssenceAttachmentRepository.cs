using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IEssenceAttachmentRepository
{
    Task<EssenceAttachment?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<EssenceAttachment>> GetByEssenceIdAsync(Guid essenceId, CancellationToken ct = default);
    Task AddAsync(EssenceAttachment attachment, CancellationToken ct = default);
    Task DeleteAsync(EssenceAttachment attachment, CancellationToken ct = default);
}