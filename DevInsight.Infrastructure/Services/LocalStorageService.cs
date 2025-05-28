using System.IO;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DevInsight.Infrastructure.Services;

public class LocalStorageService : IStorageService
{
    private readonly string _storagePath;
    private readonly IHostEnvironment _env;
    private readonly IConfiguration _configuration;

    public LocalStorageService(IHostEnvironment env, IConfiguration configuration)
    {
        _env = env;
        _configuration = configuration;
        _storagePath = Path.Combine(_env.ContentRootPath,
            _configuration["LocalStorage:Path"] ?? "LocalStorage");

        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string containerName, string fileName)
    {
        var containerPath = Path.Combine(_storagePath, containerName);
        if (!Directory.Exists(containerPath))
            Directory.CreateDirectory(containerPath);

        var filePath = Path.Combine(containerPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"{containerName}/{fileName}";
    }

    public Task<string> GetFileUrlAsync(string filePath)
    {
        var physicalPath = Path.Combine(_storagePath, filePath);
        if (!File.Exists(physicalPath))
            throw new FileNotFoundException("Arquivo não encontrado");

        var url = $"/storage/{filePath}";
        return Task.FromResult(url);
    }

    public Task<bool> DeleteFileAsync(string filePath)
    {
        var physicalPath = Path.Combine(_storagePath, filePath);
        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<Stream> DownloadFileAsync(string filePath)
    {
        var physicalPath = Path.Combine(_storagePath, filePath);
        if (!File.Exists(physicalPath))
            throw new FileNotFoundException("Arquivo não encontrado");

        return Task.FromResult((Stream)File.OpenRead(physicalPath));
    }

    public Task<string> GeneratePresignedUrlAsync(string filePath, TimeSpan expirationTime)
    {
        // Para local storage, retornamos a URL normal
        return GetFileUrlAsync(filePath);
    }
}