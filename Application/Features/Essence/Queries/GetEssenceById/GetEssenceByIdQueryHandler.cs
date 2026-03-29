using Contracts.Tasks;
using Contracts.User;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Essence.Queries.GetEssenceById;

public class GetEssenceByIdQueryHandler : IRequestHandler<GetEssenceByIdQuery, EssenceDto>
{
    private readonly IEssenceRepository _essenceRepository;
    private readonly IUserRepository _userRepository;

    public GetEssenceByIdQueryHandler(IEssenceRepository essenceRepository, IUserRepository userRepository)
    {
        _essenceRepository = essenceRepository;
        _userRepository = userRepository;
    }

    public async Task<EssenceDto> Handle(GetEssenceByIdQuery request, CancellationToken cancellationToken)
    {
        var essence = await _essenceRepository.GetByIdAsync(request.EssenceId, cancellationToken);

        if (essence is null)
            throw new ApplicationException("Essence not found");

        var rawCreator = await _userRepository.GetUserByIdAsync(essence.CreatedById, cancellationToken);
        var creator = new UserDto
        {
            Id = rawCreator!.Id,
            Email = rawCreator.Email,
            Name = rawCreator.Name,
            Phone = rawCreator.Phone,
            Surname = rawCreator.Surname,
            UserName = rawCreator.UserName,
        };

        var rawExecutor = await _userRepository.GetUserByIdAsync(essence.CreatedById, cancellationToken);
        var executor = new UserDto
        {
            Id = rawExecutor!.Id,
            Email = rawExecutor.Email,
            Name = rawExecutor.Name,
            Phone = rawExecutor.Phone,
            Surname = rawExecutor.Surname,
            UserName = rawExecutor.UserName,
        };

        var stagesDto = essence.Stages.Select(s => new StageDto(s.Id, s.EssenceId, s.Name, s.Order, s.Status,
            s.StartedAt, s.CompletedAt, s.EstimatedDuration, s.TimeSpent)).ToArray();

        return new EssenceDto
        {
            Id = essence.Id,
            Title = essence.Title,
            Description = essence.Description,
            CreatedAtUtc = essence.CreatedAtUtc,
            Priority = essence.Priority,
            Status = essence.Status,
            CreatedById = essence.CreatedById,
            DueDate = essence.DueDate,
            AssignedToId = essence.AssignedToId,
            CompletedAtUtc = essence.CompletedAtUtc,
            TimeTracked = essence.TotalTime,
            Creator = creator,
            Executor = executor,
            Stages = stagesDto
        };
    }
}