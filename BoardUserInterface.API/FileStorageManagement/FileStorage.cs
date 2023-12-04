﻿using BoardUserInterface.API.FileStorageManagement.Models;
using System.Text.Json;

namespace BoardUserInterface.API.FileStorageManagement;

public interface IFileStorage
{
    List<FileTemplateInformation> Read();
    void Save(FileTemplateInformation fileData);

    string GetLatestVersionNumber();
    void RemoveFile(string fileName);

    void RemoveAllFiles();
    string GetLastFileName();
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

    public string GetLastFileName()
    {

        var result = Read();

        if (result is null || !result.Any())
        {
            return "";
        }

        return result.Last().FileName;
    }

    public void RemoveFile(string fileName)
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

            // Optionally, log the action if you have a logger available
            //_logger.LogInformation("All files in the /Resources/Template directory have been deleted.");
        }
        else
        {
            // Optionally, log a warning or throw an exception if the directory does not exist
            //_logger.LogWarning("The directory /Resources/Template does not exist.");
        }


        // Clear the versions.json by saving an empty list
        var emptyList = new List<FileTemplateInformation>();
        var json = JsonSerializer.Serialize(emptyList);
        File.WriteAllText(filePath, json); // Assuming FilePath is publicly accessible
    }
}
