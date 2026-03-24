using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Essence.Commands.UpdateStatusPriority;

public class UpdateStatusPriorityCommandHandler: IRequestHandler<UpdateStatusPriorityCommand, Guid>
{
    private readonly IEssenceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStatusPriorityCommandHandler(IEssenceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(UpdateStatusPriorityCommand request, CancellationToken cancellationToken)
    {
        var existingEssence = await _repository.GetByIdAsync(request.EssenceId, cancellationToken);

        if (existingEssence is null)
            throw new ApplicationException("Essence not found");

        if (request.Priority is not null)
            existingEssence.ChangePriority((EEssencePriority)request.Priority);
        
        _repository.Update(existingEssence);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return existingEssence.Id;
    }
}