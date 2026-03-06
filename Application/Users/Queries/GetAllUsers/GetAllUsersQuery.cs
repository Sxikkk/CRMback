using Contracts.User;
using MediatR;

namespace Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery: IRequest<List<UserDto>>
{
    
}