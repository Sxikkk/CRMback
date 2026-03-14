
using Contracts.Tasks;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Essence.Queries.GetEssenceById;

public class GetEssenceByIdQueryHandler: IRequestHandler<GetEssenceByIdQuery, EssenceDto>
{
    private readonly IEssenceRepository _essenceRepository;

    public GetEssenceByIdQueryHandler(IEssenceRepository essenceRepository)
    {
        _essenceRepository = essenceRepository;
    }

    public async Task<EssenceDto> Handle(GetEssenceByIdQuery request, CancellationToken cancellationToken)
    {
        var essence = await _essenceRepository.GetByIdAsync(request.EssenceId, cancellationToken);

        if (essence is null)
            throw new ApplicationException("Essence not found");
        
        return new EssenceDto{
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
            TimeTracked = essence.TimeTracked,
        };
    }
}