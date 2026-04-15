using MediatR;

namespace Application.Features.EssenceAttachment.Commands.AddAttachment;
public sealed record AddEssenceAttachmentCommand(
    Guid EssenceId,
    string FileName,
    long FileSize,
    string ContentType,
    Stream FileStream
) : IRequest<Guid>;