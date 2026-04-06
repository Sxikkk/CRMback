using Domain.Interfaces.Repositories;
using MediatR;
using ApplicationException = Application.Common.Exceptions.ApplicationException;

namespace Application.Features.Essence.Commands.UpdateEssenceDueDate;

public class UpdateEssenceDueDateCommandHandler : IRequestHandler<UpdateEssenceDueDateCommand, Guid>
{
    private readonly IEssenceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEssenceDueDateCommandHandler(IEssenceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(UpdateEssenceDueDateCommand request, CancellationToken cancellationToken)
    {
        var essence = await _repository.GetByIdAsync(request.essenceId, cancellationToken);

        if (essence is null)
            throw new ApplicationException("Essence not found");
        
        essence.SetDueDate(request.dueDate);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return essence.Id;
    }
}