using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.EssenceStages.Commands.AddStage;

public class AddStageCommandHandler : IRequestHandler<AddStageCommand, Guid>
{
    private readonly IUnitOfWork _uow;
    private readonly IEssenceRepository _essenceRepository;
    private readonly IUserRepository _userRepository;

    public AddStageCommandHandler(IUnitOfWork uow, IEssenceRepository essenceRepository, IUserRepository userRepository)
    {
        _uow = uow;
        _essenceRepository = essenceRepository;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(AddStageCommand request, CancellationToken cancellationToken)
    {
        var essence = await _essenceRepository.GetByIdAsync(request.EssenceId, cancellationToken);

        if (essence is null)
            throw new ApplicationException("Essence not found");

        if (request.ResponsibleId.HasValue)
        {
            var exists = await _userRepository.IsExistByIdAsync(request.ResponsibleId.Value, cancellationToken);
            if (!exists)
                throw new ApplicationException("Responsible user not found");
        }

        TimeSpan? estimatedDuration = request.EstimatedDurationMinutes.HasValue
            ? TimeSpan.FromMinutes(request.EstimatedDurationMinutes.Value)
            : null;

        essence.AddStage(
            name: request.Name,
            order: request.Order,
            estimatedDuration: estimatedDuration);
        
        _essenceRepository.Update(essence);
        await _uow.SaveChangesAsync(cancellationToken);

        return essence.Id;
    }
}