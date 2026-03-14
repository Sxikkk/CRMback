using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;
using ApplicationException = Application.Common.Exceptions.ApplicationException;

namespace Application.Features.Essence.Commands.DeleteEssence;

public class DeleteEssenceCommandHandler: IRequestHandler<DeleteEssenceCommand, bool>
{
    private readonly IEssenceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRequestContext _requestContext;
    
    public DeleteEssenceCommandHandler(IEssenceRepository repository, IUnitOfWork unitOfWork, IRequestContext requestContext)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _requestContext = requestContext;
    }

    public async Task<bool> Handle(DeleteEssenceCommand request, CancellationToken cancellationToken)
    {
        var existingEssence = await _repository.GetByIdAsync(request.EssenceId, cancellationToken);

        if (existingEssence is null)
            throw new ApplicationException("Essence not found");

        if (existingEssence.CreatedById != _requestContext.UserId)
            throw new AccessDeniedException("Access denied");
        
        _repository.Remove(existingEssence);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}