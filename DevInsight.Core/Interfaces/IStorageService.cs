using Microsoft.AspNetCore.Http;

namespace DevInsight.Core.Interfaces.Services;

public interface IStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string containerName, string fileName);
    Task<string> GetFileUrlAsync(string filePath);
    Task<bool> DeleteFileAsync(string filePath);
    Task<Stream> DownloadFileAsync(string filePath);
    Task<string> GeneratePresignedUrlAsync(string filePath, TimeSpan expirationTime);
}