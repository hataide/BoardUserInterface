using BoardUserInterface.API.FileStorageManagement.Models;
using BoardUserInterface.API.UploadFiles;
using BoardUserInterface.API.Utils.Helpers;
using System.Net.Http.Headers;

namespace BoardUserInterface.API.Services;

public interface IFileUploadService
{
    Task<string> UploadFileAsync(IFormFile file);
}

public class FileUploadService : IFileUploadService
{
    private readonly ILogger<FileUploadService> _logger;
    private readonly IExcelMetadataService _excelMetadataService;
    private readonly List<FileTemplateInformation> _fileTemplateInformations;

    public FileUploadService(ILogger<FileUploadService> logger, IExcelMetadataService excelMetadataService)
    {
        _logger = logger;
        _excelMetadataService = excelMetadataService;
        _fileTemplateInformations = new List<FileTemplateInformation>();
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        /*
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty or null.");
        }

        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

        if (fileExtension != ".xlsx" && fileExtension != ".csv")
        {
            throw new ArgumentException("Invalid file extension. Only .xlsx and .csv files are allowed.");
        }
        */
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

        // After saving the file, extract the version number for .xlsx files
        /*
        string versionNumber = null;
        if (fileExtension == ".xlsx")
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                versionNumber = _excelMetadataService.GetVersionNumberFromExcel(memoryStream);
            }
        }
        */

        _logger.LogInformation($"File uploaded successfully: {fileName}");
        return fullPath;
    }
}
