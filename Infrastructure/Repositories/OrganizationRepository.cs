using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrganizationRepository: IOrganizationRepository
{
    private readonly AppDbContext _context;

    public OrganizationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Organization>> GetAllOrganizationsAsync(CancellationToken cancellationToken)
    {
        return await _context.Organizations.ToListAsync(cancellationToken);
    }

    public async Task<Organization?> GetOrganizationByIdAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        return await _context.Organizations.FirstOrDefaultAsync(org => org.Id == organizationId, cancellationToken);
    }

    public async Task<bool> HasOrganizationAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        return await _context.Organizations.AnyAsync(org => org.Id == organizationId, cancellationToken);
    }
    
    public async Task<List<Organization>> GetOrganizationsByIdsAsync(List<Guid>? ids, CancellationToken cancellationToken)
    {
        if (ids is not null || ids!.Count == 0)
            return [];

        return await _context.Organizations
            .Where(o => ids.Contains(o.Id))
            .ToListAsync(cancellationToken);
    }
}