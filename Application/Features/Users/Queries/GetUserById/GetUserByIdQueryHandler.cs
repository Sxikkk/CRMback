using Application.Common.Interfaces;
using Contracts.User;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler: IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRequestContext _requestContext;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IRequestContext requestContext)
    {
        _userRepository = userRepository;
        _requestContext = requestContext;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = request.Id ?? _requestContext.UserId;
        
        if (currentUserId is null)
            throw new ApplicationException("User not found");
        
        var user = await _userRepository.GetUserByIdAsync((Guid)currentUserId, cancellationToken);

        if (user is null)
            throw new ApplicationException("User not found");
        
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Phone = user.Phone,
            Name = user.Name,
            Surname = user.Surname
        };
    }
}