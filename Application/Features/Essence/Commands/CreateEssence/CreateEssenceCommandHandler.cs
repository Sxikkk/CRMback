using Application.Common.Interfaces;
using Contracts.Tasks;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Essence.Commands.CreateEssence;

public class CreateEssenceCommandHandler : IRequestHandler<CreateEssenceCommand, EssenceDto>
{
    private readonly IEssenceRepository _repository;
    private readonly IRequestContext _requestContext;
    private readonly IUnitOfWork _uow;

    public CreateEssenceCommandHandler(IEssenceRepository repository, IRequestContext requestContext, IUnitOfWork uow)
    {
        _repository = repository;
        _requestContext = requestContext;
        _uow = uow;
    }

    public async Task<EssenceDto> Handle(CreateEssenceCommand request, CancellationToken cancellationToken)
    {
        if (_requestContext.UserId is null)
            throw new ApplicationException("User not found");

        var essence = Domain.Entities.Essence.Create(request.Title, (Guid)_requestContext.UserId);
        await _repository.AddAsync(essence, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

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
            TimeTracked = essence.TimeTracked,
        };
    }
}
