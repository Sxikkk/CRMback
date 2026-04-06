using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Essence.Commands.UpdateEssenceDetails;

public class UpdateEssenceDetailsCommandHandler: IRequestHandler<UpdateEssenceDetailsCommand, Guid>
{
    private readonly IEssenceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEssenceDetailsCommandHandler(IEssenceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(UpdateEssenceDetailsCommand request, CancellationToken cancellationToken)
    {
        var existingEssence = await _repository.GetByIdAsync(request.id, cancellationToken);

        if (existingEssence is null)
            throw new ApplicationException("Essence not found");

        if (request.description is not null)
            existingEssence.UpdateDescription(request.description);
    
        existingEssence.UpdateTitle(request.title);
        
        _repository.Update(existingEssence);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return existingEssence.Id;
    }
}