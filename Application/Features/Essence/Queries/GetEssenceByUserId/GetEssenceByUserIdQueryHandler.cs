using Application.Common.Interfaces;
using Contracts.EssenceAttachment;
using Contracts.Tasks;
using Contracts.User;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Essence.Queries.GetEssenceByUserId;

public class GetEssenceByUserIdQueryHandler : IRequestHandler<GetEssenceByUserIdQuery, IReadOnlyList<EssenceDto>>
{
    private readonly IEssenceRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IEssenceAttachmentRepository _attachmentRepository;
    private readonly IRequestContext _context;

    public GetEssenceByUserIdQueryHandler(IEssenceRepository repository, IRequestContext context,
        IUserRepository userRepository, IEssenceAttachmentRepository attachmentRepository)
    {
        _repository = repository;
        _context = context;
        _userRepository = userRepository;
        _attachmentRepository = attachmentRepository;
    }

    public async Task<IReadOnlyList<EssenceDto>> Handle(GetEssenceByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? _context.UserId;

        if (userId is null)
            throw new ApplicationException("UserId is required");

        var essences = await _repository.GetByCreatorAsync((Guid)userId, cancellationToken);
        var rawCreator = await _userRepository.GetUserByIdAsync((Guid)userId, cancellationToken);
        var creator = new UserDto
        {
            Id = rawCreator!.Id,
            Email = rawCreator.Email,
            Name = rawCreator.Name,
            Phone = rawCreator.Phone,
            Surname = rawCreator.Surname,
            UserName = rawCreator.UserName,
        };

        var executorIds = essences
            .Where(e => e.AssignedToId.HasValue)
            .Select(e => e.AssignedToId!.Value)
            .Distinct()
            .ToList();

        var executors = new Dictionary<Guid, UserDto>();
        foreach (var executorId in executorIds)
        {
            var rawExecutor = await _userRepository.GetUserByIdAsync(executorId, cancellationToken);
            if (rawExecutor != null)
            {
                executors[executorId] = new UserDto
                {
                    Id = rawExecutor.Id,
                    Email = rawExecutor.Email,
                    Name = rawExecutor.Name,
                    Phone = rawExecutor.Phone,
                    Surname = rawExecutor.Surname,
                    UserName = rawExecutor.UserName,
                };
            }
        }

        var attachments = new Dictionary<Guid, List<EssenceAttachmentDto>>();
        foreach (var essence in essences)
        {
            attachments[essence.Id] = [];
            var rawAttachments = await _attachmentRepository.GetByEssenceIdAsync(essence.Id, cancellationToken);

            foreach (var rawAtt in rawAttachments)
            {
                var uploader = await _userRepository.GetUserByIdAsync(rawAtt.UploadedById, cancellationToken);

                if (uploader is null)
                    throw new ApplicationException("User with such id doesn't exist");
                    
                attachments[essence.Id].Add(new EssenceAttachmentDto
                {
                    Id = rawAtt.Id,
                    ContentType = rawAtt.ContentType,
                    FileName = rawAtt.FileName,
                    FilePath = rawAtt.FilePath,
                    Size = rawAtt.Size,
                    UploadedBy = new UserDto
                    {
                        Id = uploader.Id,
                        Email = uploader.Email,
                        Name = uploader.Name,
                        Phone = uploader.Phone,
                        Surname = uploader.Surname,
                        UserName = uploader.UserName,
                    },
                    UploadedAtUtc = rawAtt.UploadedAtUtc,
                });
            }
        }

        return essences.Select(essence => new EssenceDto
        {
            Id = essence.Id,
            Title = essence.Title,
            Description = essence.Description,
            CreatedAtUtc = essence.CreatedAtUtc,
            Priority = essence.Priority,
            Status = essence.Status,
            CreatedById = essence.CreatedById,
            DueDate = essence.DueDate,
            AssignedToId = essence.AssignedToId,
            CompletedAtUtc = essence.CompletedAtUtc,
            TimeTracked = essence.TotalTime,
            Creator = creator,
            Price = essence.EssencePrice?.Value,
            Stages = essence.Stages.Select(s => new StageDto(s.Id, s.EssenceId, s.Name, s.Order, s.Status,
                s.StartedAt, s.CompletedAt, s.EstimatedDuration, s.TimeSpent)).ToArray(),
            Executor = essence.AssignedToId.HasValue &&
                       executors.TryGetValue(essence.AssignedToId.Value, out var executor)
                ? executor
                : null,
            Attachments = attachments[essence.Id].ToArray(),
        }).ToList();
    }
}