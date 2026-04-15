namespace Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, Guid essenceId, CancellationToken ct = default);
    Task DeleteFileAsync(string relativePath, CancellationToken cancellationToken = default);
}