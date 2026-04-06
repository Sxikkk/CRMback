using Application.Common.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;
using ApplicationException = Application.Common.Exceptions.ApplicationException;

namespace Application.Features.Users.Commands.ChangeUserPassword;

public sealed class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, Guid>
{
    private readonly IRequestContext _requestContext;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeUserPasswordCommandHandler(
        IRequestContext requestContext,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _requestContext = requestContext;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? _requestContext.UserId;

        if (userId is null)
            throw new ApplicationException("User not found");

        var user = await _userRepository.GetUserByIdAsync(userId.Value, cancellationToken);

        if (user is null)
            throw new ApplicationException("User not found");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new ApplicationException("Current password is invalid");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.ChangePassword(hashedPassword);

        _userRepository.UpdateUser(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
