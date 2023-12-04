using BoardUserInterface.API.FileStorageManagement.Models;
using System.Text.Json;

namespace BoardUserInterface.API.FileStorageManagement;

public interface IFileStorage
{
    List<FileTemplateInformation> Read();
    void Save(FileTemplateInformation fileData);
    FileTemplateInformation GetLatestFile();

    string GetLatestVersionNumber();
}

public class FileStorage : IFileStorage
{
    private readonly string filePath;

    public FileStorage(string filePath)
    {
        this.filePath = filePath;
    }

    public List<FileTemplateInformation> Read()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<FileTemplateInformation>>(json);
        }

        return new List<FileTemplateInformation>();
    }

    public void Save(FileTemplateInformation fileData)
    {
        var result = Read();
        List<FileTemplateInformation> fileTemplate = new();

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

    public FileTemplateInformation GetLatestFile()
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
