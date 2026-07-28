using FileUploadService.Models;

namespace FileUploadService.Repositories;

public interface IFileRepository
{
    Task<IEnumerable<UploadedFile>> GetAllAsync();
    Task<UploadedFile?> GetByIdAsync(Guid id);
    Task AddAsync(UploadedFile file);
    Task UpdateAsync(UploadedFile file);
    Task DeleteAsync(Guid id);
}