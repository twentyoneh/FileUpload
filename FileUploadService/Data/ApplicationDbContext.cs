using FileUploadService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileUploadService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<UploadedFile> Files { get; set; }
}