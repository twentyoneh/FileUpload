using FileUploadService.DTO;
using FileUploadService.Exceptions;
using FileUploadService.Models;
using FileUploadService.Repositories;
using FileUploadService.Storage;
using Microsoft.AspNetCore.StaticFiles;

namespace FileUploadService.Services;

public class FileService(IFileRepository fileRepository, IFileStorage fileStorage) : IFileService
{
    public async Task<IEnumerable<FileResponseDto>> GetAllFilesAsync()
    {
        var files = await fileRepository.GetAllAsync();

        return files.Select(p => new FileResponseDto
        {
            Id = p.Id,
            FileName = p.FileName,
            FileExtension = p.FileExtension,
            FileSize = p.FileSize
        });
    }

    public async Task<FileResponseDto> GetFileByIdAsync(Guid id)
    {
        var file = await fileRepository.GetByIdAsync(id);

        if (file == null)
            throw new KeyNotFoundException("File not found");

        return new FileResponseDto
        {
            Id = file.Id,
            FileName = file.FileName,
            FileExtension = file.FileExtension,
            FileSize = file.FileSize
        };
    }

    public async Task<FileResponseDto> UploadFileAsync(IFormFile file) //file123.txt
    {
        
        if (file == null || file.Length == 0)
        {
            throw new FileUploadException("File is empty");
        }

        if (file.Length > 52428800)
        {
            throw new FileUploadSizeException("File is too large");
        }
        
        var fileId = Guid.NewGuid(); // генерация ID 3819274
        var fileName = fileId + Path.GetExtension(file.FileName); // 3819274.txt

        
        await using (var stream = file.OpenReadStream())
        {
            await fileStorage.UploadAsync(fileName, stream);
        }

        var uploadedFile = new UploadedFile
        {
            Id = fileId,
            FileName = Path.GetFileNameWithoutExtension(file.FileName),
            FileExtension = Path.GetExtension(file.FileName),
            FileSize = file.Length,
            UpdateAt = DateTime.UtcNow,
            UploadedAt = DateTime.UtcNow
        };
        await fileRepository.AddAsync(uploadedFile);
        
        return new FileResponseDto()
        {
            Id = uploadedFile.Id,
            FileName = uploadedFile.FileName,
            FileExtension = uploadedFile.FileExtension,
            FileSize = uploadedFile.FileSize
        };
    }

    public async Task<FileResponseDto> UpdateFileAsync(Guid id, FileRequestDto fileDto)
    { 
        var file = await fileRepository.GetByIdAsync(id);
        if(file == null)
            throw new KeyNotFoundException("File not found");
        file.FileName = fileDto.FileName;
        file.UpdateAt = DateTime.UtcNow;
        await fileRepository.UpdateAsync(file);
        return new FileResponseDto()
        {
            Id = file.Id,
            FileName = fileDto.FileName,
            FileExtension = file.FileExtension,
            FileSize = file.FileSize
        };
    }

    public async Task DeleteFileAsync(Guid id)
    {
        var file = await fileRepository.GetByIdAsync(id);
        
        if(file == null)
            throw new KeyNotFoundException("File not found");
        
        var fileName = id + file.FileExtension;

        try
        {
            await fileStorage.DeleteAsync(fileName);
        }
        catch (Exception e)
        {
            if (e is FileNotFoundException)
                throw new KeyNotFoundException("File not found", e);

            throw;
        }
        await fileRepository.DeleteAsync(id);
    }

    public async Task<FileDownloadDTO> DownloadFileAsync(Guid id)
    {
        var file = await fileRepository.GetByIdAsync(id);
        
        if(file == null)
            throw new KeyNotFoundException("File not found");
        
        var fileName = id + file.FileExtension;
        var fileExists = await fileStorage.ExistsAsync(fileName);
        
        if(!fileExists)
            throw new FileNotFoundException("File not found");
        
        var fs = await fileStorage.OpenReadAsync(fileName);
        var provider = new FileExtensionContentTypeProvider();

        var contentTypeExists = provider.TryGetContentType(fileName, out var contentType);
        var displayContentType = contentTypeExists ? contentType : "application/octet-stream";

        file.DownloadAt = DateTime.UtcNow;
        await fileRepository.UpdateAsync(file);
        
        return new FileDownloadDTO()
        {
            Stream = fs,
            ContentType = displayContentType,
            FileExtension = file.FileExtension,
            FileName =  file.FileName,
        };
    }
}