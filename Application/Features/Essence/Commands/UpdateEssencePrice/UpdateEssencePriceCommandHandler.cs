using Domain.Interfaces.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Essence.Commands.UpdateEssencePrice;

public class UpdateEssencePriceCommandHandler: IRequestHandler<UpdateEssencePriceCommand, Guid>
{
    private readonly IEssenceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEssencePriceCommandHandler(IEssenceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(UpdateEssencePriceCommand request, CancellationToken cancellationToken)
    {
        var essence = await _repository.GetByIdAsync(request.essenceId, cancellationToken);

        if (essence is null)
            throw new ApplicationException("Essence not found");

        if (request.price is not null)
        {
            var newPrice = new EssencePrice((decimal)request.price);
            essence.SetPrice(newPrice);
        }
        else
        {
            essence.SetPrice(null);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return essence.Id;
    }
}