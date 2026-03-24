using Contracts.Organization;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Organization.Queries.GetAllOrganizations;

public class GetAllOrganizationsQueryHandler : IRequestHandler<GetAllOrganizationsQuery, IList<SimpleOrganizationDto>>
{
    private readonly IOrganizationRepository _repository;

    public GetAllOrganizationsQueryHandler(IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<SimpleOrganizationDto>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var rawOrganizations = await _repository.GetAllOrganizationsAsync(cancellationToken);

        return rawOrganizations.Select((org) => new SimpleOrganizationDto
        {
            Id = org.Id,
            Name = org.Name,
            Description = CreateDescription(org.Inn ?? "ИНН не задан", org.Ogrn  ?? "ОГРН не задан", org.Type, org.Phone  ?? "Номер не назначен")
        }).ToList();
    }

    private string CreateDescription(string inn, string ogrn, EOrganizationType orgType, string orgPhone) =>
        $"{inn} - {orgType} - {ogrn} - {orgPhone}";
}