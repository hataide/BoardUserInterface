using BoardUserInterface.API.FileStorageManagement.Models;
using System.Text.Json;

namespace BoardUserInterface.API.FileStorageManagement;

public interface IRepositoryStorage
{
    List<RepositoryTemplateInformation> Read();
    void Save(RepositoryTemplateInformation fileData);

    string GetLatestVersionNumber();
    void RemoveFile(string fileName);

    void RemoveAllFiles();
    string GetLastFileName();

    RepositoryTemplateInformation GetLatestFile();
}

public class RepositoryStorage : IRepositoryStorage
{
    private readonly string filePath;

    public RepositoryStorage(string filePath)
    {
        this.filePath = filePath;
    }

    public List<RepositoryTemplateInformation> Read()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<RepositoryTemplateInformation>>(json);
        }

        return new List<RepositoryTemplateInformation>();
    }

    public void Save(RepositoryTemplateInformation fileData)
    {
        var result = Read();
        List<RepositoryTemplateInformation> fileTemplate = new();

        if (result is null)
        {
            fileTemplate.Add(fileData);   
        }
        else
        {
            result.Add(fileData);
            fileTemplate = result;
        }

        var json = JsonSerializer.Serialize(fileTemplate);
        File.WriteAllText(filePath, json);
    }

    
    public string GetLatestVersionNumber()
    {
        var result = Read();

        if(result is null || !result.Any() )
        {
            return "0.0";
        }

        return result.Last().VersionNumber;
    }

    public string GetLastFileName()
    {

        var result = Read();

        if (result is null || !result.Any())
        {
            return string.Empty;
        }

        return result.Last().FileName;
    }

    public void RemoveFile(string fileName)
    {
        try
        {
            // Now, remove the corresponding entry from the versions.json
            var allFiles = Read();
            var fileToRemove = allFiles.FirstOrDefault(f => f.FileName == fileName);

            if (fileToRemove != null)
            {
                allFiles.Remove(fileToRemove);
                var json = JsonSerializer.Serialize(allFiles);
                File.WriteAllText(filePath, json);
            }
        }
        catch (Exception ex)
        {
            // Rethrow the exception to be handled by the caller
            throw new Exception($"Failed to remove '{fileName}' from repository.", ex);
        }
    }
    
    public void RemoveAllFiles()
    {
        // Clear the versions.json by saving an empty list
        var emptyList = new List<RepositoryTemplateInformation>();
        var json = JsonSerializer.Serialize(emptyList);
        File.WriteAllText(filePath, json); // Assuming FilePath is publicly accessible
    }

    public RepositoryTemplateInformation GetLatestFile()
    {
        var files = Read();
        if (files == null || !files.Any())
        {
            throw new FileNotFoundException("No files available.");
        }

        // Sort the files by VersionNumber in descending order and select the first one
        var latestFile = files.OrderByDescending(f => Version.Parse(f.VersionNumber)).FirstOrDefault();
        if (latestFile == null)
        {
            throw new FileNotFoundException("No files available.");
        }

        return latestFile;
    }
}
