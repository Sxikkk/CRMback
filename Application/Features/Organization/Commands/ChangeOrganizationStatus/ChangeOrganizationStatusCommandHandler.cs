using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Organization.Commands.ChangeOrganizationStatus;

public class ChangeOrganizationStatusCommandHandler: IRequestHandler<ChangeOrganizationStatusCommand, Guid>
{
    private readonly IOrganizationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrganizationStatusCommandHandler(IOrganizationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(ChangeOrganizationStatusCommand request, CancellationToken cancellationToken)
    {
        var organization = await _repository.GetOrganizationByIdAsync(request.organizationId, cancellationToken);

        if (organization is null)
            throw new ApplicationException("Organization not found");

        switch (request.status)
        {
            case EOrganizationStatus.Active:
                organization.Activate();
                break;
            case EOrganizationStatus.Blocked:
                organization.Block();
                break;
            default:
                throw new ApplicationException("Unknown status, status not changed");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return request.organizationId;
    }
}