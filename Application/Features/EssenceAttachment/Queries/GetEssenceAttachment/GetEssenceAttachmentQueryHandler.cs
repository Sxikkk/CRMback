using Contracts.EssenceAttachment;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.EssenceAttachment.Queries.GetEssenceAttachment;

public class GetEssenceAttachmentQueryHandler : IRequestHandler<GetEssenceAttachmentQuery, EssenceAttachmentDto>
{
    private readonly IEssenceAttachmentRepository _repository;

    public GetEssenceAttachmentQueryHandler(IEssenceAttachmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<EssenceAttachmentDto?> Handle(GetEssenceAttachmentQuery request,
        CancellationToken cancellationToken)
    {
        var attachmentData = await _repository.GetByIdAsync(request.attachmentId, cancellationToken);
        
        if (attachmentData is null) return null;

        return new EssenceAttachmentDto
        {
            Id = attachmentData.Id,
            ContentType = attachmentData.ContentType,
            FileName = attachmentData.FileName,
            FilePath = attachmentData.FilePath,
            Size = attachmentData.Size,
            UploadedAtUtc = attachmentData.UploadedAtUtc,
        };
    }
}