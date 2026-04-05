using IstGuide.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IstGuide.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;

    public FileStorageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default)
    {
        if (fileStream == null || fileStream.Length == 0)
            throw new ArgumentException("Dosya boş olamaz.");

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
        
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{fileName.Replace(" ", "_")}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var destStream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(destStream, ct);
        }

        return $"/uploads/{uniqueFileName}";
    }

    public Task DeleteFileAsync(string fileUrl, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(fileUrl)) return Task.CompletedTask;

        var path = fileUrl.TrimStart('/');
        var fullPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), path);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public string GetPublicUrl(string filePath)
    {
        return filePath;
    }
}
