using Amazon.S3;
using Amazon.S3.Model;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using DeleteObjectRequest = Amazon.S3.Model.DeleteObjectRequest;
using GetObjectRequest = Amazon.S3.Model.GetObjectRequest;
using PutObjectRequest = Amazon.S3.Model.PutObjectRequest;

namespace DevInsight.Infrastructure.Services;

public class StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;
    private readonly ILogger<StorageService> _logger;
    private readonly string _bucketName;

    public StorageService(
        IAmazonS3 s3Client,
        IConfiguration configuration,
        ILogger<StorageService> logger)
    {
        _s3Client = s3Client;
        _configuration = configuration;
        _logger = logger;
        _bucketName = _configuration["AWS:BucketName"] ?? "devinsight-uploads";
    }

    public async Task<string> UploadFileAsync(IFormFile file, string containerName, string fileName)
    {
        try
        {
            var filePath = $"{containerName}/{fileName}";

            using var fileStream = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath,
                InputStream = fileStream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.Private
            };

            await _s3Client.PutObjectAsync(request);

            _logger.LogInformation("Arquivo {FilePath} enviado com sucesso para o bucket {BucketName}",
                filePath, _bucketName);

            return filePath;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar arquivo para o S3");
            throw new ApplicationException("Erro ao enviar arquivo para o armazenamento", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao enviar arquivo");
            throw;
        }
    }

    public async Task<string> GetFileUrlAsync(string filePath)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = filePath,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Protocol = Protocol.HTTPS
            };

            var url = _s3Client.GetPreSignedURL(request);
            _logger.LogDebug("URL gerada para o arquivo {FilePath}: {Url}", filePath, url);

            return url;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar URL para o arquivo {FilePath}", filePath);
            throw new ApplicationException("Erro ao gerar URL do arquivo", ex);
        }
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath
            };

            await _s3Client.DeleteObjectAsync(request);
            _logger.LogInformation("Arquivo {FilePath} excluído com sucesso do bucket {BucketName}",
                filePath, _bucketName);

            return true;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir arquivo {FilePath}", filePath);
            return false;
        }
    }

    public async Task<Stream> DownloadFileAsync(string filePath)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath
            };

            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao baixar arquivo {FilePath}", filePath);
            throw new ApplicationException("Erro ao baixar arquivo", ex);
        }
    }

    public async Task<string> GeneratePresignedUrlAsync(string filePath, TimeSpan expirationTime)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = filePath,
                Expires = DateTime.UtcNow.Add(expirationTime),
                Protocol = Protocol.HTTPS
            };

            return _s3Client.GetPreSignedURL(request);
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar URL assinada para {FilePath}", filePath);
            throw new ApplicationException("Erro ao gerar URL assinada", ex);
        }
    }
}