using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Essence.Commands.UpdateAssignedToEssence;

public class UpdateAssignedToEssenceCommandHandler: IRequestHandler<UpdateAssignedToEssenceCommand, Guid>
{
    private readonly IEssenceRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAssignedToEssenceCommandHandler(IEssenceRepository repository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(UpdateAssignedToEssenceCommand request, CancellationToken cancellationToken)
    {
        var existingEssence = await _repository.GetByIdAsync(request.EssenceId, cancellationToken);
        
        if (existingEssence is null)
            throw new ApplicationException("Essence not found");
        
        if (request.AssignedToId is null)
            existingEssence.AssignTo(request.AssignedToId);
            
        if (await _userRepository.IsExistByIdAsync((Guid)request.AssignedToId!, cancellationToken) is false)
            throw new ApplicationException("User to assign not found");

        existingEssence.AssignTo(request.AssignedToId);
        
        _repository.Update(existingEssence);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return existingEssence.Id;
    }
}