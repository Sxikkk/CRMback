using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrganizationRepository
{
    Task<List<Organization>> GetAllOrganizationsAsync(CancellationToken cancellationToken);
    Task<Organization?> GetOrganizationByIdAsync(Guid organizationId, CancellationToken cancellationToken);
    Task<bool> HasOrganizationAsync(Guid organizationId, CancellationToken cancellationToken);
    Task<List<Organization>> GetOrganizationsByIdsAsync(List<Guid>? ids, CancellationToken cancellationToken);
    Task AddOrganizationAsync(Organization organization, CancellationToken cancellationToken);
    Task<bool> IsUniqueOrganizationAsync(CancellationToken cancellationToken, string? ogrn = null, string? inn = null);
}