using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using BoardUserInterface.FileService.Service;
using BoardUserInterface.Common.Helpers;
using BoardUserInterface.Common.Exceptions;
using Microsoft.AspNetCore.StaticFiles;
using BoardUserInterface.Repository;
using BoardUserInterface.Repository.Models;
using BoardUserInterface.FileService.Helpers.VersionValidator;
using BoardUserInterface.FileService.Helpers.ExcelMetadata;
using BoardUserInterface.FileService.Helpers.VersionComparer.VersionComparer;
using BoardUserInterface.Service.Logging;
using System;
using BoardUserInterface.Service.Http;
namespace BoardUserInterface.Service.Template;

public class TemplateService : ITemplateService
{
    private readonly IFileService _fileService;
    private readonly IRepositoryStorage _repositoryStorage;
    private readonly IExcelMetadataHelper _excelMetadataService;
    private readonly IVersionValidatorHelper _versionValidator;
    private readonly IVersionComparerHelper _versionComparer;

    private readonly ILogService _logService;

    private readonly ILogger<IFileService> _logger;

    public TemplateService(IFileService fileService, IExcelMetadataHelper excelMetadataService, IRepositoryStorage repositoryStorage, IVersionValidatorHelper versionValidator, IVersionComparerHelper versionComparer, ILogger<IFileService> logger, ILogService logService)
    {
        _fileService = fileService;
        _excelMetadataService = excelMetadataService;
        _repositoryStorage = repositoryStorage;
        _versionValidator = versionValidator;
        _versionComparer = versionComparer;

        _logService = logService;

        _logger = logger;
    }
    
    public async Task<string> Upload(IFormFile file)
    {
        var uploadedFileVersion = TryGetFileVersion(file);
        // If the version is newer, store the file and update the version number in the repository

        SaveToRepository(file.FileName, uploadedFileVersion);

        await _fileService.UploadFileAsync(file);

        return uploadedFileVersion;
    }

    private void SaveToRepository(string fileName, string uploadedFileVersion)
    {
        var newFileName = FileNameHelper.SetNewVersionFileName(fileName, uploadedFileVersion);
        _repositoryStorage.Save(new RepositoryTemplateInformation() { FileName = newFileName, VersionNumber = uploadedFileVersion });
    }

    private string TryGetFileVersion(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new EmptyFileException("File is empty or null.");
        }

        // Check if the file is an Excel file based on its content type
        if (!file.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidFileContentTypeException($"The uploaded file has an invalid content type.");
        }

        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

        if (fileExtension != ".xlsx" && fileExtension != ".csv")
        {
            throw new InvalidFileExtensionException("Invalid file extension. Only .xlsx and .csv files are allowed.");
        }

        // Extract version number from the Excel file
        string uploadedFileVersion = _excelMetadataService.GetVersionNumberFromExcel(file.OpenReadStream());

        if (!_versionValidator.IsValidVersion(uploadedFileVersion))
        {
            throw new InvalidVersionException($"The version [{uploadedFileVersion}] of the uploaded file is not valid.");
        }

        if (string.IsNullOrEmpty(uploadedFileVersion))
        {
            throw new NoVersionException($"The uploaded Excel file does not contain a version number in its metadata.");
        }

        // Retrieve the latest version number from repository
        string latestVersion = _repositoryStorage.GetLatestVersionNumber();

        // Compare versions
        if (!_versionComparer.CompareVersions(uploadedFileVersion, latestVersion))
        {
            throw new OldVersionException($"Already has a template with that version or a newer one.");
        }

        return uploadedFileVersion;
    }


    public (string fileName, string version) RemoveLastVersion()
    {
        try
        {
            // Retrieve the latest version number from the repository
            string latestVersion = _repositoryStorage.GetLatestVersionNumber();

            // Retrive the last file name from the repository
            string fileName = _repositoryStorage.GetLastFileName();
            if (string.IsNullOrEmpty(fileName))
            {
                throw new NoVersionException("No version found to remove.");
            }

            // Remove from the repo
            _repositoryStorage.RemoveFile(fileName);
            // Remove file from system
            _fileService.RemoveFile(fileName);

            _logService.LogMessage("Backend", "Successful", $"Last version of {fileName} was removed successfully", "Information");

            //_logger.LogInformation($"Last version of {fileName} was removed successfully");

            return (fileName, latestVersion);

        }
        catch
        {
            throw new NoVersionException("Failed to remove the last version of the Excel file.");
        }
    }

    public List<(string filename, string version)> RemoveAllVersionsAsync()
    {
        try
        {
            var files = _repositoryStorage.Read();
            // Remove all files from repo
            _repositoryStorage.RemoveAllFiles();
            // Remove all files from system
            _fileService.RemoveAllFiles();

            _logService.LogMessage("Backend", "Successful", "All versions were removed successfully", "Information");
            //_logger.LogInformation($"All versions were removed successfully");

            return files.Select(p => (p.FileName, p.VersionNumber)).ToList();
        }
        catch
        {
            throw new NoVersionException("Failed to remove the last version of the Excel file.");
        }
    }

    public (string fileContentBase64, string contentType, string fileName) DownloadLatestFile()
    {
        try
        {
            var latestFile = _repositoryStorage.GetLatestFile();
            var folderName = Path.Combine("Resources", "Template");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName, latestFile.FileName);
            var fileContent = File.ReadAllBytes(filePath);

            var fileContentBase64 = Convert.ToBase64String(fileContent);

            // Use FileExtensionContentTypeProvider to determine the content type
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(latestFile.FileName, out var contentType))
            {
                contentType = "application/octet-stream"; // Default content type if none is found
            }

            _logService.LogMessage("Backend", "Successful", "Download successful", "Information");
            return (fileContentBase64, contentType, latestFile.FileName);
        }
        catch
        {
            throw new FileNotFoundException("The requested file is not available.");
        }
    }
}
