namespace Contracts.Organization;

public record SimpleOrganizationDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
};