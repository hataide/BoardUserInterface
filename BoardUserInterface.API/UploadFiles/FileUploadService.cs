using BoardUserInterface.API.FileStorageManagement.Models;
using BoardUserInterface.API.UploadFiles;
using BoardUserInterface.API.Utils.Helpers;
using System.Net.Http.Headers;

namespace BoardUserInterface.API.Services;

public interface IFileUploadService
{
    Task UploadFileAsync(IFormFile file);
}

public class FileUploadService : IFileUploadService
{
    private readonly ILogger<FileUploadService> _logger;
    private readonly IExcelMetadataService _excelMetadataService;

    public FileUploadService(ILogger<FileUploadService> logger, IExcelMetadataService excelMetadataService)
    {
        _logger = logger;
        _excelMetadataService = excelMetadataService;
    }

    public async Task UploadFileAsync(IFormFile file)
    {
        
        var folderName = Path.Combine("Resources", "Template");
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        var fileVersion = _excelMetadataService.GetVersionNumberFromExcel(file.OpenReadStream());
        var fileName = FileNameHelper.SetNewVersionFileName(file.FileName, fileVersion);
        var fullPath = Path.Combine(filePath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        _logger.LogInformation($"File uploaded successfully: {fileName}");
    }
}
