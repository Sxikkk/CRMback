using Application.Common.Interfaces;
using Contracts.Tasks;
using Contracts.User;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Essence.Commands.CreateEssence;

public class CreateEssenceCommandHandler : IRequestHandler<CreateEssenceCommand, EssenceDto>
{
    private readonly IEssenceRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IRequestContext _requestContext;
    private readonly IUnitOfWork _uow;

    public CreateEssenceCommandHandler(IEssenceRepository repository, IRequestContext requestContext, IUnitOfWork uow, IUserRepository userRepository)
    {
        _repository = repository;
        _requestContext = requestContext;
        _uow = uow;
        _userRepository = userRepository;
    }

    public async Task<EssenceDto> Handle(CreateEssenceCommand request, CancellationToken cancellationToken)
    {
        if (_requestContext.UserId is null)
            throw new ApplicationException("User not found");

        var rawCreator = await _userRepository.GetUserByIdAsync((Guid)_requestContext.UserId, cancellationToken);
        
        if (rawCreator is null)
            throw new ApplicationException("Failed to create Essence. Creator not found");

        if (rawCreator?.OrganizationId is null)
            throw new ApplicationException("Failed to create Essence. Organization not found");
        
        var essence = Domain.Entities.Essence.Create(request.Title, (Guid)_requestContext.UserId, (Guid)rawCreator.OrganizationId);
        await _repository.AddAsync(essence, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
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
            Stages = [],
            Price = essence.EssencePrice?.Value
        };
    }
}
