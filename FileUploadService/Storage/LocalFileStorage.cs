namespace FileUploadService.Storage;

public class LocalFileStorage : IFileStorage
{
    private static readonly string UploadFolder = "FilesUploadedLocally";
    private static readonly string PathUploadFolder = Path.Combine(AppContext.BaseDirectory, UploadFolder);
    
    
    public Task<bool> ExistsAsync(string fileName)
    {
        var fileFullPath = Path.Combine(PathUploadFolder, fileName);
        
        return Task.FromResult(File.Exists(fileFullPath));
    }

    public async Task UploadAsync(string fileName, Stream fileStream)
    {
        Directory.CreateDirectory(PathUploadFolder);
        var fileFullName = Path.Combine(PathUploadFolder, fileName);
        await using var file = File.Create(fileFullName);
        await fileStream.CopyToAsync(file);
        
    }

    public Task DeleteAsync(string fileName)
    {
        var fileFullPath = Path.Combine(PathUploadFolder, fileName);
        File.Delete(fileFullPath);
        return Task.CompletedTask;
    }

    public Task<Stream> OpenReadAsync(string fileName)
    {
        var fileFullPath = Path.Combine(PathUploadFolder, fileName);
        return Task.FromResult<Stream>(File.OpenRead(fileFullPath));
    }
}