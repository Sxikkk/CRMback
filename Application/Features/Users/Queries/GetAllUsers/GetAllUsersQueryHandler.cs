using Contracts.Organization;
using Contracts.User;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler: IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _repository;
    private readonly IOrganizationRepository _organizationRepository;

    public GetAllUsersQueryHandler(IUserRepository repository, IOrganizationRepository organizationRepository)
    {
        _repository = repository;
        _organizationRepository = organizationRepository;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        // Получаем всех пользователей
        var users = await _repository.GetAllUsersAsync(cancellationToken);
        var enumerableUsers = users.ToList();

        // Получаем уникальные Id организаций, исключая null
        var organizationIds = enumerableUsers
            .Where(u => u.OrganizationId.HasValue)
            .Select(u => u.OrganizationId!.Value)
            .Distinct()
            .ToList();

        // Получаем все организации одним запросом
        var organizationsList = await _organizationRepository.GetOrganizationsByIdsAsync(organizationIds, cancellationToken);

        // Формируем словарь для быстрого доступа
        var organizationsDict = organizationsList.ToDictionary(
            o => o.Id,
            o => new SimpleOrganizationDto
            {
                Id = o.Id,
                Name = o.Name,
                Description = Domain.Entities.Organization.CreateDescription(
                    o.Inn, o.Ogrn, o.Type, o.Phone
                )
            });

        // Формируем DTO пользователей
        var result = enumerableUsers.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            Phone = u.Phone,
            Name = u.Name,
            Surname = u.Surname,
            Organization = u.OrganizationId.HasValue && organizationsDict.TryGetValue(u.OrganizationId.Value, out var org)
                ? org
                : null
        }).ToList();

        return result;
    }
}