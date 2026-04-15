using Application.Common.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;
using ApplicationException = Application.Common.Exceptions.ApplicationException;

namespace Application.Features.EssenceAttachment.Commands.AddAttachment;

public class AddEssenceAttachmentCommandHandler : IRequestHandler<AddEssenceAttachmentCommand, Guid>
{
    private readonly IEssenceRepository _repository;
    private readonly IEssenceAttachmentRepository _attachmentRepository;
    private readonly IRequestContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IUnitOfWork _unitOfWork;

    public AddEssenceAttachmentCommandHandler(IEssenceRepository repository, IRequestContext context,
        IFileStorageService fileStorage, IUnitOfWork unitOfWork, IEssenceAttachmentRepository attachmentRepository)
    {
        _repository = repository;
        _context = context;
        _fileStorage = fileStorage;
        _unitOfWork = unitOfWork;
        _attachmentRepository = attachmentRepository;
    }

    public async Task<Guid> Handle(AddEssenceAttachmentCommand request, CancellationToken ct)
    {
        if (_context.UserId is null)
            throw new ApplicationException("User Id is null");

        var essence = await _repository.GetByIdAsync(request.EssenceId, ct);
        if (essence == null)
            throw new ApplicationException("Essence not found");

        var relativePath =
            await _fileStorage.SaveFileAsync(request.FileStream, request.FileName, request.EssenceId, ct);

        var attachment = Domain.Entities.EssenceAttachment.Create(essence.Id, request.FileName, relativePath, request.FileSize,
            request.ContentType, (Guid)_context.UserId);

        await _attachmentRepository.AddAsync(attachment, ct);
        
        essence.AddAttachment(attachment);

        await _unitOfWork.SaveChangesAsync(ct);

        return essence.Id;
    }
}