using Contracts.EssenceAttachment;
using MediatR;

namespace Application.Features.EssenceAttachment.Queries.GetEssenceAttachment;

public record GetEssenceAttachmentQuery(Guid attachmentId): IRequest<EssenceAttachmentDto?>;