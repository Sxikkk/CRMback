using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.Repositories;

public interface IEssenceRepository
{
    Task<Essence?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Essence>> GetByCreatorAsync(Guid creatorId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Essence>> GetByAssignedUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Essence>> GetByStatusAsync(EEssenceStatus status, CancellationToken cancellationToken = default);
    
    Task AddAsync(Essence essence, CancellationToken cancellationToken = default);

    void Update(Essence essence);

    void Remove(Essence essence);
    Task<IReadOnlyDictionary<EEssenceStatus, int>> GetStatusStatisticByCreatorSqlAsync(                                                                                                                                                                                                                       
        Guid creatorId,                                                                                                                                                                                                                                                                                       
        CancellationToken cancellationToken = default);         
    Task<IReadOnlyList<Essence>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default);
}