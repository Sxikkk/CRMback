using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrganizationRepository
{
    Task<List<Organization>> GetAllOrganizationsAsync(CancellationToken cancellationToken);
}