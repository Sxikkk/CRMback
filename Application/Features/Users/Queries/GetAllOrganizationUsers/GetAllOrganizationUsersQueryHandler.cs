using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetAllOrganizationUsers;

public class GetAllOrganizationUsersQueryHandler: IRequestHandler<GetAllOrganizationUsersQuery, IEnumerable<User>>
{
    private readonly IUserRepository _userRepository;

    public GetAllOrganizationUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> Handle(GetAllOrganizationUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersByOrgIdAsync(request.organizationId, cancellationToken);
        
        return users.ToList();
    }
}