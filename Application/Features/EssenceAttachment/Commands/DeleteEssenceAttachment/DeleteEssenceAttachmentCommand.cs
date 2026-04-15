using MediatR;

namespace Application.Features.Essence.Commands.DeleteEssenceAttachment;

public sealed record DeleteEssenceAttachmentCommand(Guid attachmentId): IRequest<Guid>;