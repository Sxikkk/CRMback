using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Organization.Commands.ChangeOrganizationType;

public class ChangeOrganizationTypeCommandHandler: IRequestHandler<ChangeOrganizationTypeCommand, Guid>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public ChangeOrganizationTypeCommandHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(ChangeOrganizationTypeCommand request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetOrganizationByIdAsync(request.organizationId, cancellationToken);

        if (organization is null)
            throw new ApplicationException("Organization not found");
        
        organization.ChangeType(request.Type);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return request.organizationId;
    }
}