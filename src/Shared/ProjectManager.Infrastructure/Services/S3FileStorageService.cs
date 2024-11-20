using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ProjectManager.Infrastructure.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string readerName);
}

public class S3FileStorageService(IAmazonS3 s3Client, IConfiguration configuration) : IFileStorageService
{
    private readonly string _bucketName = configuration["AWS:S3:BucketName"];

    public async Task<string> UploadFileAsync(IFormFile file, string readerName)
    {
        var fileName = file.FileName;
        var objectKey = $"TaskAttachments/{readerName}/{fileName}";

        await using var fileToUpload = file.OpenReadStream();
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            InputStream = fileToUpload,
            ContentType = file.ContentType
        };

        var response = await s3Client.PutObjectAsync(putObjectRequest); 
        return GeneratePreSignedUrl(objectKey);
    }
    
    private string GeneratePreSignedUrl(string objectKey)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddHours(24)
        };

        return s3Client.GetPreSignedURL(request);
    }
}