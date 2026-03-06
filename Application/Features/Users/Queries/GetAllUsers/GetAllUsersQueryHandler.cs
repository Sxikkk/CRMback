using Contracts.User;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler: IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _repository;

    public GetAllUsersQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllUsersAsync();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            Phone = u.Phone,
            Name = u.Name,
            Surname = u.Surname,
        }).ToList();
    }
}