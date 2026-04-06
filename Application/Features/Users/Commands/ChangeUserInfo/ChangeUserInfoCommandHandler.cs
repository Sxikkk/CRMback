using Application.Common.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects;
using MediatR;
using ApplicationException = Application.Common.Exceptions.ApplicationException;

namespace Application.Features.Users.Commands.ChangeUserInfo;

public sealed class ChangeUserInfoCommandHandler : IRequestHandler<ChangeUserInfoCommand, Guid>
{
    private readonly IRequestContext _requestContext;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeUserInfoCommandHandler(
        IRequestContext requestContext,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _requestContext = requestContext;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(ChangeUserInfoCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? _requestContext.UserId;

        if (userId is null)
            throw new ApplicationException("User not found");

        var user = await _userRepository.GetUserByIdAsync(userId.Value, cancellationToken);

        if (user is null)
            throw new ApplicationException("User not found");

        if (!string.IsNullOrWhiteSpace(request.Name))
            user.ChangeName(request.Name);

        if (!string.IsNullOrWhiteSpace(request.Surname))
            user.ChangeSurname(request.Surname);

        if (!string.IsNullOrWhiteSpace(request.UserName) &&
            !string.Equals(user.UserName, request.UserName, StringComparison.Ordinal))
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(request.UserName, cancellationToken);

            if (existingUser is not null && existingUser.Id != user.Id)
                throw new ApplicationException("Username is already taken");

            user.ChangeUserName(request.UserName);
        }

        if (!string.IsNullOrWhiteSpace(request.Email) &&
            !string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (existingUser is not null && existingUser.Id != user.Id)
                throw new ApplicationException("Email is already taken");

            user.ChangeEmail(Email.Create(request.Email));
        }

        if (!string.IsNullOrWhiteSpace(request.Phone) &&
            !string.Equals(user.Phone, request.Phone, StringComparison.Ordinal))
        {
            user.ChangePhone(Phone.Create(request.Phone));
        }

        _userRepository.UpdateUser(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
