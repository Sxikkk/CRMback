using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Organization.Commands.CreateOrganization;

public class CreateOrganizationCommandHandler: IRequestHandler<CreateOrganizationCommand, Guid>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateOrganizationCommandHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        if (!await _organizationRepository.IsUniqueOrganizationAsync(cancellationToken, request.Inn, request.Ogrn))
            throw new ApplicationException("Organization is not unique");        
        
        var organization = Domain.Entities.Organization.Create(request.OrganizationName, request.OrganizationType);

        if (request.Inn is not null)
            organization.SetInn(request.Inn);

        if (request.Ogrn is not null)
            organization.SetOgrn(request.Ogrn);

        if (request.Email is not null || request.Phone is not null)
            organization.SetContact(request.Email, request.Phone);

        if (request.Website is not null)
            organization.SetWebsite(request.Website);
        
        await _organizationRepository.AddOrganizationAsync(organization, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return organization.Id;
    }
}