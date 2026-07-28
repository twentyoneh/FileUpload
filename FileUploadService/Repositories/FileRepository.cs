using FileUploadService.Data;
using FileUploadService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileUploadService.Repositories;

public class FileRepository : IFileRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public FileRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<UploadedFile>> GetAllAsync()
    {
        return await _dbContext.Files.ToListAsync();
    }

    public async Task<UploadedFile?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Files.FindAsync(id);
    }

    public async Task AddAsync(UploadedFile file)
    {
        await _dbContext.Files.AddAsync(file);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(UploadedFile file)
    {
        _dbContext.Update(file);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var file = await _dbContext.Files.FindAsync(id);
        if (file != null)
        {
            _dbContext.Remove(file);
            await _dbContext.SaveChangesAsync();
        }
    }
}