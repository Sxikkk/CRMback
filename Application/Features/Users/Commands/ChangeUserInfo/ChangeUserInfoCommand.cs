using MediatR;

namespace Application.Features.Users.Commands.ChangeUserInfo;

public sealed record ChangeUserInfoCommand(
    Guid? UserId,
    string? Name,
    string? Surname,
    string? UserName,
    string? Email,
    string? Phone
) : IRequest<Guid>;
