using MediatR;

namespace Application.Features.Users.Commands.ChangeUserPassword;

public sealed record ChangeUserPasswordCommand(
    Guid? UserId,
    string CurrentPassword,
    string NewPassword
) : IRequest<Guid>;
