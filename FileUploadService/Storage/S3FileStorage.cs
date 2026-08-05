using Amazon.S3;
using Amazon.S3.Model;
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
            
            await _s3Client.GetObjectMetadataAsync(request); //выбросит исключение AmazonS3Exception

            return true;
        }
        catch (AmazonS3Exception ex )
        {
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
            Key = fileName,
            InputStream = fileStream
        };
        
        await _s3Client.PutObjectAsync(request);
    }

    public async Task DeleteAsync(string fileName)
    {
        try
        {
            var deleteObjRequest = new DeleteObjectRequest
            {
                BucketName = _s3Options.Value.BucketName, 
                Key = fileName
            };
            await _s3Client.DeleteObjectAsync(deleteObjRequest);
        } 
        catch (AmazonS3Exception ex )
        {
            if (string.Equals(ex.ErrorCode, "NotFound"))
                throw new FileNotFoundException("File not found", fileName);

            throw;
        }
        
    }

    public Task<Stream> OpenReadAsync(string fileName)
    {
        throw new NotImplementedException();
    }
}