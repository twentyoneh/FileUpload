namespace FileUploadService.Models;

public class UploadedFile
{
    public Guid Id { get; set; }
    public string? FileName { get; set; }
    public string? FileExtension { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime? DownloadAt { get; set; }

}