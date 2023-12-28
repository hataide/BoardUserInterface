using BoardUserInterface.Common.Exceptions;
using BoardUserInterface.Common.Helpers;
using BoardUserInterface.FileService.Helpers.ExcelMetadata;
using BoardUserInterface.FileService.Helpers.VersionComparer.VersionComparer;
using BoardUserInterface.FileService.Helpers.VersionValidator;
using BoardUserInterface.FileService.Service;
using BoardUserInterface.Helpers;
using BoardUserInterface.Repository;
using BoardUserInterface.Repository.Models;
using BoardUserInterface.Service.Auditing;
using BoardUserInterface.Service.DataAccess;
using BoardUserInterface.Service.Logging;
using BoardUserInterface.Service.Models;
using BoardUserInterfaces.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;
namespace BoardUserInterface.Service.Template;

public class TemplateService : ITemplateService
{
    private readonly IFileService _fileService;
    private readonly IRepositoryStorage _repositoryStorage;
    private readonly IExcelMetadataHelper _excelMetadataService;
    private readonly IVersionValidatorHelper _versionValidator;
    private readonly IVersionComparerHelper _versionComparer;
    private readonly ILogService _logService;
    private readonly IAuditService _auditService;

    private readonly IFilesAuditRepoService _filesAuditRepoService;
    private readonly ILogsRepoService _logsRepoService;

    public TemplateService(IFileService fileService, IExcelMetadataHelper excelMetadataService, IRepositoryStorage repositoryStorage, IVersionValidatorHelper versionValidator, IVersionComparerHelper versionComparer, ILogService logService, IAuditService auditService, IFilesAuditRepoService filesAuditRepoService, ILogsRepoService logsRepoService)
    {
        _fileService = fileService;
        _excelMetadataService = excelMetadataService;
        _repositoryStorage = repositoryStorage;
        _versionValidator = versionValidator;
        _versionComparer = versionComparer;
        _logService = logService;
        _auditService = auditService;

        _filesAuditRepoService = filesAuditRepoService;
        _logsRepoService = logsRepoService;

    }

    public async Task<string> Upload(IFormFile file)
    {
        var uploadedFileVersion = TryGetFileVersion(file);

        // string lastFile = ""; // _repositoryStorage.GetLatestFile();
        // If the version is newer, store the file and update the version number in the repository
        SaveToRepository(file.FileName, uploadedFileVersion);

        await _fileService.UploadFileAsync(file);

        var filesAuditRecord = new FilesAudit
        {

            Timestamp = DateTime.UtcNow,
            Action = "CREATE",
            RecordId = 1,
            Content_old = null,
            Content_new = null,
            Version_old = "0.0",
            Version_new = uploadedFileVersion,
            Name_old = "",
            Name_new = file.FileName,
            Extension_old = "",
            Extension_new = file.ContentType
        };
        _ = await _filesAuditRepoService.CreateFilesAuditAsync(filesAuditRecord);

        var newLog = new Logs
        {
            Source = "Backend",
            Context = "Successful",
            Message = $"Last version of {file.FileName} was uploaded successfully",
            Type = "Information"
        };
        _logsRepoService.CreateLogAsync(newLog);
        
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


    public async Task<(string fileName, string version)> RemoveLastVersion()
    {
        try
        {
            // Retrieve the latest version number from the repository
            string latestVersion = null;//_repositoryStorage.GetLatestVersionNumber();
            var lastFile = _repositoryStorage.GetLatestFile();

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
          

            var filesAuditRecord = new FilesAudit
            {

                Timestamp = DateTime.UtcNow,
                Action = "DELETE",
                RecordId = 1,
                Content_old = null,
                Content_new = null,
                Version_old = lastFile.VersionNumber,
                Version_new = "",
                Name_old = lastFile.FileName,
                Name_new = "",
                Extension_old = "",
                Extension_new = ""
            };


            // Record the audit event
            _filesAuditRepoService.CreateFilesAuditAsync(filesAuditRecord);

            _logService.LogMessage("TESTING AUDIT", "Successful", JsonConverterHelper.SerializeObject(filesAuditRecord) , "Information");

            var newLog = new Logs
            {
                Source = "Backend",
                Context = "Successful",
                Message = $"Last version of {fileName} was removed successfully",
                Type = "Information"
            };
            _= await _logsRepoService.CreateLogAsync(newLog);

            return (fileName, latestVersion);

        }
        catch
        {
            throw new NoVersionException("Failed to remove the last version of the Excel file.");
        }
    }

    public async Task<List<(string filename, string version)>> RemoveAllVersionsAsync()
    {
        try
        {
            var lastFile = _repositoryStorage.GetLatestFile();
            var files = _repositoryStorage.Read();
            // Remove all files from repo
            _repositoryStorage.RemoveAllFiles();
            // Remove all files from system
            _fileService.RemoveAllFiles();

            var filesAuditRecord = new FilesAudit
            {

                Timestamp = DateTime.UtcNow,
                Action = "DELETEALL",
                RecordId = 1,
                Content_old = null,
                Content_new = null,
                Version_old = lastFile.VersionNumber,
                Version_new = "",
                Name_old = lastFile.FileName,
                Name_new = "",
                Extension_old = "",
                Extension_new = ""
            };


            // Record the audit event
            _filesAuditRepoService.CreateFilesAuditAsync(filesAuditRecord);

            _logService.LogMessage("Backend", "Successful", "All versions were removed successfully", "Information");

            var newLog = new Logs
            {
                Source = "Backend",
                Context = "Successful",
                Message = "All versions were removed successfully",
                Type = "Information"
            };
            _= await _logsRepoService.CreateLogAsync(newLog);

            return files.Select(p => (p.FileName, p.VersionNumber)).ToList();
        }
        catch
        {
            throw new NoVersionException("Failed to remove the last version of the Excel file.");
        }
    }

    public  (string fileContentBase64, string contentType, string fileName) DownloadLatestFile()
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

            // Create an audit record for the download action
            var auditRecord = new AuditRecord
            {
                Timestamp = DateTime.UtcNow,
                Action = "DownloadLatestFile",
                TableName = "Templates",
                RecordId = latestFile.FileName, // Use the name of the downloaded file as the record identifier
                NewValues = $"Downloaded file version: {latestFile.VersionNumber}",
                OldValues = string.Empty // No old values for a download action
            };

            // Record the audit event
            _auditService.RecordAuditAsync(auditRecord);

            _logService.LogMessage("Backend", "Successful", "Download successful", "Information");

            var newLog = new Logs
            {
                Source = "Backend",
                Context = "Successful",
                Message = "Download successful",
                Type = "Information"
            };
            _logsRepoService.CreateLogAsync(newLog);

            return (fileContentBase64, contentType, latestFile.FileName);
        }
        catch
        {
            throw new FileNotFoundException("The requested file is not available.");
        }
    }
}
