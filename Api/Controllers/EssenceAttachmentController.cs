using Application.Features.Essence.Commands.DeleteEssenceAttachment;
using Application.Features.EssenceAttachment.Commands.AddAttachment;
using Application.Features.EssenceAttachment.Queries.GetEssenceAttachment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/attachments")]
[Authorize]
public class EssenceAttachmentController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;

    public EssenceAttachmentController(IMediator mediator, IWebHostEnvironment environment)
    {
        _mediator = mediator;
        _environment = environment;
    }

    [HttpPost("{essenceId:guid}")]
    public async Task<IActionResult> UploadAttachmentAsync(
        Guid essenceId, 
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        await using var stream = file.OpenReadStream();

        var command = new AddEssenceAttachmentCommand(
            EssenceId: essenceId,
            FileName: file.FileName,
            FileSize: file.Length,
            ContentType: file.ContentType,
            FileStream: stream
        );
        
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("{attachmentId:guid}")]
    public async Task<IActionResult> UploadAttachmentAsync(
        [FromRoute] Guid attachmentId, 
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new DeleteEssenceAttachmentCommand(attachmentId), cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("{attachmentId:guid}")]
    public async Task<IActionResult> DownloadAttachmentAsync(
        Guid attachmentId,
        CancellationToken cancellationToken)
    {
        var attachment = await _mediator.Send(
            new GetEssenceAttachmentQuery(attachmentId),
            cancellationToken);

        if (attachment is null)
        {
            await Console.Error.WriteLineAsync("Attachment not found");
            return NotFound();
        }

        var fullPath = Path.Combine(
            _environment.WebRootPath,
            attachment.FilePath);

        if (!System.IO.File.Exists(fullPath))
        {
            await Console.Error.WriteLineAsync($"Attachment not exist, {fullPath} - {attachment.FilePath}");
            return NotFound();
        }
        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);

        return File(stream, attachment.ContentType, attachment.FileName);
    }
}