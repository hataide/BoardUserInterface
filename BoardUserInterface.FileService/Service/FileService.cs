using BoardUserInterface.Common.Exceptions;
using BoardUserInterface.Common.Helpers;
using BoardUserInterface.FileService.Helpers.ExcelMetadata;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BoardUserInterface.FileService.Service;



public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IExcelMetadataHelper _excelMetadataService;

    public FileService(ILogger<FileService> logger, IExcelMetadataHelper excelMetadataService)
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

    public void RemoveFile(string fileName)
    {
        try
        {
            // First, construct the full path of the file to be removed
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Template", fileName);

            // Check if the file exists before attempting to delete it
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("File not found.", fileName);
            }

            // Attempt to delete the file from the file system
            File.Delete(fullPath);
        }
        catch
        {
            throw new RemoveFileException($"Exception removing file {fileName}");
        }
    }

    public void RemoveAllFiles()
    {
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Template");

        // Check if the directory exists
        if (Directory.Exists(directoryPath))
        {
            // Get all file names within the directory
            var files = Directory.GetFiles(directoryPath);

            // Loop through each file and delete it
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
        else
        {
            throw new DirectoryEmpyException("There are no files to be removed.");
        }
    }
}
