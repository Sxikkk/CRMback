using Domain.Enums;

namespace Application.Common.Interfaces;

public interface IRequestContext
{
    string IpAddress { get; }
    Guid? UserId { get; }
    string? Username { get; }
}