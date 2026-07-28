namespace FileUploadService.DTO;

public record FileDownloadDTO()
{
    public Stream? Stream { get; init; }
    public string? ContentType  { get; init; }
    public string? FileExtension { get; init; }
    public string? FileName { get; init; }

};