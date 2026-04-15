using Contracts.Organization;
using Contracts.Tasks;
using Contracts.User;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Organization.Queries.GetOrganizationInfo;

public class GetOrganizationInfoQueryHandler : IRequestHandler<GetOrganizationInfoQuery, FullOrganizationDto>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IEssenceRepository _essenceRepository;
    private readonly IUserRepository _userRepository;

    public GetOrganizationInfoQueryHandler(IOrganizationRepository organizationRepository,
        IUserRepository userRepository, IEssenceRepository essenceRepository)
    {
        _organizationRepository = organizationRepository;
        _userRepository = userRepository;
        _essenceRepository = essenceRepository;
    }

    public async Task<FullOrganizationDto> Handle(GetOrganizationInfoQuery request, CancellationToken cancellationToken)
    {
        var orgInfo = await _organizationRepository.GetOrganizationByIdAsync(request.organizationId, cancellationToken);
        if (orgInfo is null) throw new ApplicationException("Organization not found");

        var users = (await _userRepository.GetAllUsersByOrgIdAsync(request.organizationId, cancellationToken)).ToList();
        var userDict = users.ToDictionary(u => u.Id, u => new UserDto
        {
            Id = u.Id, Name = u.Name, Surname = u.Surname, Email = u.Email,
            Phone = u.Phone, UserName = u.UserName
        });

        var essences = await _essenceRepository.GetByOrganizationIdAsync(request.organizationId, cancellationToken);

        var essencesDto = essences.Select(e => new EssenceDto
        {
            Id = e.Id,
            AssignedToId = e.AssignedToId,
            CreatedById = e.CreatedById,
            CompletedAtUtc = e.CompletedAtUtc,
            CreatedAtUtc = e.CreatedAtUtc,
            Creator = userDict.GetValueOrDefault(e.CreatedById) ?? throw new ApplicationException("Creator not found"),
            Executor = e.AssignedToId.HasValue
                ? userDict.GetValueOrDefault(e.AssignedToId.Value) ??
                  throw new ApplicationException("Executor not found")
                : null,
            Description = e.Description,
            DueDate = e.DueDate,
            Priority = e.Priority,
            Status = e.Status,
            TimeTracked = e.TotalTime,
            Title = e.Title,
            Price = e.EssencePrice?.Value,
            Stages = e.Stages.Select(s => new StageDto(
                s.Id,
                s.EssenceId,
                s.Name,
                s.Order,
                s.Status,
                s.StartedAt,
                s.CompletedAt,
                s.EstimatedDuration,
                s.TimeSpent
            )).ToArray(),
        }).ToList();

        var usersDto = users.Select(u => userDict[u.Id]).ToList();

        return new FullOrganizationDto
        (
            orgInfo.Id,
            orgInfo.Name,
            orgInfo.Inn,
            orgInfo.Ogrn,
            orgInfo.Email,
            orgInfo.Phone,
            orgInfo.Website,
            orgInfo.City,
            orgInfo.CreateDate,
            orgInfo.Type,
            orgInfo.Status,
            usersDto,
            essencesDto
        );
    }
}