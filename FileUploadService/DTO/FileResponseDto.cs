namespace FileUploadService.DTO;

public record FileResponseDto
{
    public Guid Id { get; init; }
    public string? FileName { get; init; }
    public string? FileExtension { get; init; }
    public long FileSize { get; init; }
    
}