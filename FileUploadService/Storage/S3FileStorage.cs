using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FileUploadService.Config;
using Microsoft.Extensions.Options;

namespace FileUploadService.Storage;

public class S3FileStorage(IAmazonS3 s3Client, IOptions<S3Options> options) : IFileStorage
{
    private readonly IOptions<S3Options> _s3Options = (options);
    private readonly IAmazonS3 _s3Client = (s3Client);

    public async Task<bool> ExistsAsync(string fileName)
    {
        try
        {
            GetObjectMetadataRequest request = new GetObjectMetadataRequest
            {
                BucketName = _s3Options.Value.BucketName,
                Key = fileName
            };
            
            var response = await _s3Client.GetObjectMetadataAsync(request);

            return true;
        }
        catch (AmazonS3Exception ex )
        {
            
            if (string.Equals(ex.ErrorCode, "NoSuchBucket"))
                return false;
            
            if (string.Equals(ex.ErrorCode, "NotFound"))
                return false;

            throw;
        }
    }

    public async Task UploadAsync(string fileName, Stream fileStream)
    {
        
        
        var request = new PutObjectRequest
        {
            BucketName = _s3Options.Value.BucketName,
            Key = fileName, //тут должен быть id
            InputStream = fileStream
        };
        
        await _s3Client.PutObjectAsync(request);
    }

    public Task DeleteAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> OpenReadAsync(string fileName)
    {
        throw new NotImplementedException();
    }
}