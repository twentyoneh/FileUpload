using FileUploadService.DTO;
using FileUploadService.Exceptions;
using FileUploadService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadService.Controllers;

[Route("api/[controller]")]
public class FilesController : Controller
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        try
        {
            var file = await _fileService.DownloadFileAsync(id);
            
            return File(file.Stream, file.ContentType,  file.FileName + file.FileExtension);
        }
        catch (Exception e)
        {
            if (e is not KeyNotFoundException && e is not FileNotFoundException) throw;
            Console.WriteLine(e);
            return NotFound(e.Message);

        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var files = await _fileService.GetAllFilesAsync();
        return Ok(files);
    }

    [HttpGet("{id}")] // GET /api/file/45
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var file = await _fileService.GetFileByIdAsync(id);
            return Ok(file);
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> Update(Guid id, FileRequestDto fileDto)
    {
        try
        {
            var updatedFile = await _fileService.UpdateFileAsync(id, fileDto);
            return Ok(updatedFile);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            var savedFile = await _fileService.UploadFileAsync(file);
            
            return CreatedAtAction(nameof(GetById), new { id = savedFile.Id }, savedFile);
        }
        catch (FileUploadException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    // [HttpPost("presigned-upload")]
    // public async Task<IActionResult> PesingnedUpload(IFormFile file)
    // {
    //     try
    //     {
    //
    //         return CreatedAtAction()
    //     }
    //     catch (FileUploadException e)
    //     {
    //         
    //     }
    //     
    // }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _fileService.DeleteFileAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}