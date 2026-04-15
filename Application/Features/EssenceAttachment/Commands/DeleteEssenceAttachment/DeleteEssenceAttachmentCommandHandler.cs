using Application.Common.Interfaces;
using Application.Features.Essence.Commands.DeleteEssenceAttachment;
using Domain.Interfaces.Repositories;
using MediatR;
using ApplicationException = Application.Common.Exceptions.ApplicationException;

namespace Application.Features.EssenceAttachment.Commands.DeleteEssenceAttachment;

public class DeleteEssenceAttachmentCommandHandler: IRequestHandler<DeleteEssenceAttachmentCommand, Guid>
{
    private readonly IEssenceRepository _essenceRepository;
    private readonly IEssenceAttachmentRepository _essenceAttachmentRepository;
    private readonly IFileStorageService _fileStorage;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEssenceAttachmentCommandHandler(
        IEssenceRepository essenceRepository,
        IFileStorageService fileStorage, IEssenceAttachmentRepository essenceAttachmentRepository, IUnitOfWork unitOfWork)
    {
        _essenceRepository = essenceRepository;
        _fileStorage = fileStorage;
        _essenceAttachmentRepository = essenceAttachmentRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Guid> Handle(DeleteEssenceAttachmentCommand request, CancellationToken cancellationToken)
    {
        var attachment = await _essenceAttachmentRepository.GetByIdAsync(request.attachmentId, cancellationToken);
        if (attachment == null) 
            throw new ApplicationException("Attachment not found");

        await _fileStorage.DeleteFileAsync(attachment.FilePath, cancellationToken);

        var essence = await _essenceRepository.GetByIdAsync(attachment.EssenceId, cancellationToken);
        if (essence is null)
            throw new ApplicationException("Essence not found");
            
        essence.RemoveAttachment(request.attachmentId);
        await _essenceAttachmentRepository.DeleteAsync(attachment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return essence.Id;
    }
}