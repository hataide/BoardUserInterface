using BoardUserInterface.API.Controllers.V1;
using BoardUserInterface.API.Exceptions;
using BoardUserInterface.API.FileStorageManagement;
using BoardUserInterface.API.FileStorageManagement.Models;
using BoardUserInterface.API.UploadFiles;
using BoardUserInterface.API.Utils.Helpers;
using System.Net.Http.Headers;

namespace BoardUserInterface.API.Services.Template;

public interface IUploadTemplateService
{
    Task<string> Upload(IFormFile file);
}

public class UploadTemplateService : IUploadTemplateService
{
    private readonly IFileUploadService _fileUploadService;
    private readonly IFileStorage _fileStorage;
    private readonly IExcelMetadataService _excelMetadataService;
    private readonly IVersionValidator _versionValidator;
    private readonly IVersionComparer _versionComparer;

    public UploadTemplateService(IFileUploadService fileUploadService, IExcelMetadataService excelMetadataService, IFileStorage fileStorage, IVersionValidator versionValidator, IVersionComparer versionComparer)
    {
        _fileUploadService = fileUploadService;
        _excelMetadataService = excelMetadataService;
        _fileStorage = fileStorage;
        _versionValidator = versionValidator;
        _versionComparer = versionComparer;

    }
    public async Task<string> Upload(IFormFile file)
    {
        var uploadedFileVersion = TryGetFileVersion(file);
        // If the version is newer, store the file and update the version number in the repository


        SaveToRepository(file.FileName, uploadedFileVersion);

        await _fileUploadService.UploadFileAsync(file);

        return uploadedFileVersion;
    }

    private void SaveToRepository(string fileName, string uploadedFileVersion)
    {
        var newFileName = FileNameHelper.SetNewVersionFileName(fileName, uploadedFileVersion);
        _fileStorage.Save(new FileTemplateInformation() { FileName = newFileName, VersionNumber = uploadedFileVersion });
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
        string latestVersion = _fileStorage.GetLatestVersionNumber();

        // Compare versions
        if (!_versionComparer.CompareVersions(uploadedFileVersion, latestVersion))
        {
            throw new OldVersionException($"Already has a template with that version or a newer one.");
        }

        return uploadedFileVersion;
    }
}
