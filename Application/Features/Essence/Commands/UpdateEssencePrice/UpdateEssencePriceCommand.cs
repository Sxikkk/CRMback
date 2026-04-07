using MediatR;

namespace Application.Features.Essence.Commands.UpdateEssencePrice;

public record UpdateEssencePriceCommand(Guid essenceId, decimal? price): IRequest<Guid>;