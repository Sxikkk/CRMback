using Domain.Interfaces.Repositories;
using MediatR;
using ApplicationException = Application.Common.Exceptions.ApplicationException;

namespace Application.Features.EssenceStages.Commands.CompleteStage;

public sealed class CompleteStageCommandHandler : IRequestHandler<CompleteStageCommand, Guid>
{
    private readonly IEssenceRepository _essenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteStageCommandHandler(IEssenceRepository essenceRepository, IUnitOfWork unitOfWork)
    {
        _essenceRepository = essenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CompleteStageCommand request, CancellationToken cancellationToken)
    {
        var essence = await _essenceRepository.GetByIdAsync(request.essenceId, cancellationToken);

        if (essence is null)
            throw new ApplicationException("Essence not found");

        essence.CompleteStage(request.stageId);

        _essenceRepository.Update(essence);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return essence.Id;
    }
}
