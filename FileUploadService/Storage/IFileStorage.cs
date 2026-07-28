//Интерфейс для сырой работы с данными 
namespace FileUploadService.Storage;

public interface IFileStorage
{
    Task<bool> ExistsAsync(string fileName);
    Task UploadAsync(string fileName, Stream fileStream);
    Task DeleteAsync(string fileName);
    Task<Stream> OpenReadAsync(string fileName);
}