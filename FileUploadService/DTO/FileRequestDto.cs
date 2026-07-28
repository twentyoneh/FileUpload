namespace FileUploadService.DTO;

public record FileRequestDto
{
    //пока что изменение только файла 
    public string FileName { get; init; }
}