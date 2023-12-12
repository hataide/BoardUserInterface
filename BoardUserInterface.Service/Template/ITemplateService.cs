using Microsoft.AspNetCore.Http;

namespace BoardUserInterface.Service.Template;

public interface ITemplateService
{
    (string fileName, string version) RemoveLastVersion();
    List<(string filename, string version)> RemoveAllVersionsAsync();
    Task<string> Upload(IFormFile file);
    (byte[] fileContent, string contentType, string fileName) DownloadLatestFile();
}
