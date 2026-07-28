using FileUploadService.DTO;

namespace FileUploadService.Services;

public interface IFileService
{
    Task<IEnumerable<FileResponseDto>> GetAllFilesAsync();
    Task<FileResponseDto> GetFileByIdAsync(Guid id);
    Task<FileResponseDto> UploadFileAsync(IFormFile file);
    Task<FileResponseDto> UpdateFileAsync(Guid id, FileRequestDto FileDto);
    Task DeleteFileAsync(Guid id);
    Task<FileDownloadDTO> DownloadFileAsync(Guid id);
}