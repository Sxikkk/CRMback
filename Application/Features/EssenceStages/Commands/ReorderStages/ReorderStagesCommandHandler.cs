using Domain.Interfaces.Repositories;
using MediatR;
using ApplicationException = Application.Common.Exceptions.ApplicationException;

namespace Application.Features.EssenceStages.Commands.ReorderStages;

public sealed class ReorderStagesCommandHandler : IRequestHandler<ReorderStagesCommand, Guid>
{
    private readonly IEssenceRepository _essenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReorderStagesCommandHandler(IEssenceRepository essenceRepository, IUnitOfWork unitOfWork)
    {
        _essenceRepository = essenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(ReorderStagesCommand request, CancellationToken cancellationToken)
    {
        var essence = await _essenceRepository.GetByIdAsync(request.essenceId, cancellationToken);

        if (essence is null)
            throw new ApplicationException("Essence not found");

        var changes = request.changes
            .Select(change => (change.stageId, change.newOrder))
            .ToList();

        essence.ReorderStages(changes);

        _essenceRepository.Update(essence);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return essence.Id;
    }
}
