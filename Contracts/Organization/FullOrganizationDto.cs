using Contracts.Tasks;
using Contracts.User;
using Domain.Enums;

namespace Contracts.Organization;

public record FullOrganizationDto(
    Guid Id,
    string Name,
    string? Inn,
    string? Ogrn,
    string? Email,
    string? Phone,
    string? Website,
    string? City,
    DateTime CreateDate,
    EOrganizationType Type,
    EOrganizationStatus Status,
    IReadOnlyList<UserDto> Users,
    IReadOnlyList<EssenceDto> Essences
);