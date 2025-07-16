using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;
    private const string _folder = "uploads/chat-images";

    public FileStorageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file provided", nameof(file));

        // 1) Build the root path under wwwroot:
        var uploadsRoot = Path.Combine(_env.ContentRootPath, "wwwroot", _folder);
        // 2) Ensure it exists:
        Directory.CreateDirectory(uploadsRoot);

        // 3) Generate a unique filename and full filesystem path:
        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsRoot, fileName);

        // 4) Copy the incoming stream:
        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await file.CopyToAsync(stream, cancellationToken);

        // 5) Return the public URL portion (so client can use <img src="...">)
        //    Note: Make sure you have `app.UseStaticFiles()` in Program.cs
        var relativeUrl = Path.Combine("/", _folder, fileName)
                              .Replace("\\", "/");
        return relativeUrl;
    }
}
