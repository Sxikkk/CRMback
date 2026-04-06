using Domain.Interfaces.Repositories;
using MediatR;
using ApplicationException = Application.Common.Exceptions.ApplicationException;

namespace Application.Features.EssenceStages.Commands.StartStage;

public sealed class StartStageCommandHandler : IRequestHandler<StartStageCommand, Guid>
{
    private readonly IEssenceRepository _essenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartStageCommandHandler(IEssenceRepository essenceRepository, IUnitOfWork unitOfWork)
    {
        _essenceRepository = essenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(StartStageCommand request, CancellationToken cancellationToken)
    {
        var essence = await _essenceRepository.GetByIdAsync(request.essenceId, cancellationToken);

        if (essence is null)
            throw new ApplicationException("Essence not found");

        essence.StartStage(request.stageId);

        _essenceRepository.Update(essence);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return essence.Id;
    }
}
