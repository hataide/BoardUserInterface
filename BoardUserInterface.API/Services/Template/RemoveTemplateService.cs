using BoardUserInterface.API.Exceptions;
using BoardUserInterface.API.FileStorageManagement;

namespace BoardUserInterface.API.Services.Template;
public interface IRemoveTemplateService
{
    Task<string> RemoveLastVersion();
}

public class RemoveTemplateService : IRemoveTemplateService
{
    private readonly IFileStorage _fileStorage;

    public RemoveTemplateService(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
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

            return fileName;

        }
        catch
        {
            throw new NoVersionException("Failed to remove the last version of the Excel file.");
        }
    }
}
