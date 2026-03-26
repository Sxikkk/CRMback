using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrganizationRepository
{
    Task<List<Organization>> GetAllOrganizationsAsync(CancellationToken cancellationToken);
    Task<Organization?> GetOrganizationByIdAsync(Guid organizationId, CancellationToken cancellationToken);
    Task<bool> HasOrganizationAsync(Guid organizationId, CancellationToken cancellationToken);
}