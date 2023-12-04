using BoardUserInterface.API.Exceptions;
using BoardUserInterface.API.FileStorageManagement;
using BoardUserInterface.API.FileStorageManagement.Models;
using System.Text.Json;

namespace BoardUserInterface.API.Services.Template;
public interface IRemoveTemplateService
{
    Task<string> RemoveLastVersion();
    Task RemoveAllVersionsAsync();
}

public class RemoveTemplateService : IRemoveTemplateService
{
    private readonly IFileStorage _fileStorage;
    private readonly ILogger _logger;

    public RemoveTemplateService(IFileStorage fileStorage, ILogger logger)
    {
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<string> RemoveLastVersion()
    {
        try
        {
            // Retrieve the latest version number from the repository
            string latestVersion = _fileStorage.GetLatestVersionNumber();

            // Retrive the last file name from the repository
            string fileName = _fileStorage.GetLastFileName();
            if (string.IsNullOrEmpty(fileName))
            {
                throw new NoVersionException("No version found to remove.");
            }
            
            // Remove the file
            _fileStorage.RemoveFile(fileName);
            _logger.LogInformation($"Last version of {fileName} was removed successfully");

            return fileName;

        }
        catch
        {
            throw new NoVersionException("Failed to remove the last version of the Excel file.");
        }
    }

    public async Task RemoveAllVersionsAsync()
    {

        try
        {
            
            // Remove all files
            _fileStorage.RemoveAllFiles();
            _logger.LogInformation($"All versions were removed successfully");

        }
        catch
        {
            throw new NoVersionException("Failed to remove the last version of the Excel file.");
        }

    }

}
