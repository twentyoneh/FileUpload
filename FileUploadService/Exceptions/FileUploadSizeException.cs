namespace FileUploadService.Exceptions;

public class FileUploadSizeException : FileUploadException
{
    public FileUploadSizeException() { }
    public FileUploadSizeException(string message) : base(message) { }
    public FileUploadSizeException(string message, Exception inner) : base(message, inner) { }
}