using Microsoft.AspNetCore.Http;

namespace BoardUserInterface.FileService.Service;

public interface IFileService
{
    Task UploadFileAsync(IFormFile file);

    void RemoveFile(string fileName);

    void RemoveAllFiles();
}

