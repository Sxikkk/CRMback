using Application.Common.Interfaces;

namespace Api.Services;

public class FileStorageService: IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, Guid essenceId, CancellationToken ct = default)
    {
        var uniqueName = $"{Guid.NewGuid()}_{fileName}";
        var relativePath = $"attachments/essence/{essenceId}/{uniqueName}";
        
        if (string.IsNullOrEmpty(_environment.WebRootPath))
            throw new ArgumentNullException(nameof(_environment.WebRootPath));
        
        var fullPath = Path.Combine(_environment.WebRootPath, relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var fs = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(fs, ct);

        return relativePath.Replace("\\", "/");
    }

    public Task DeleteFileAsync(string relativePath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return Task.CompletedTask;

        if (string.IsNullOrEmpty(_environment.WebRootPath))
            throw new ArgumentNullException(nameof(_environment.WebRootPath));

        var fullPath = Path.Combine(_environment.WebRootPath, relativePath);

        if (!File.Exists(fullPath))
            return Task.CompletedTask;

        File.Delete(fullPath);

        return Task.CompletedTask;
    }
}