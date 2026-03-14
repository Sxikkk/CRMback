using Application.Common.Interfaces;
using Contracts.Tasks;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Essence.Queries.GetEssenceByUserId;

public class GetEssenceByUserIdQueryHandler : IRequestHandler<GetEssenceByUserIdQuery, IReadOnlyList<EssenceDto>>
{
    private readonly IEssenceRepository _repository;
    private readonly IRequestContext _context;

    public GetEssenceByUserIdQueryHandler(IEssenceRepository repository, IRequestContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IReadOnlyList<EssenceDto>> Handle(GetEssenceByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        if (_context.UserId == null)
            throw new ApplicationException("UserId is required");

        var essences = await _repository.GetByCreatorAsync((Guid)_context.UserId, cancellationToken);

        return essences.Select(essence => new EssenceDto
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
            TimeTracked = essence.TimeTracked,
        }).ToList();
    }
}