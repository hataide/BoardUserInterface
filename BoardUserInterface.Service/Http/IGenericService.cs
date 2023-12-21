using Microsoft.AspNetCore.Http;
using BoardUserInterface.Service.Http.Response;

namespace BoardUserInterface.Service.Http;

public interface IGenericService
{
    Task<DownloadResponse> TestGet();
    //Task<string> TestPost(String s); //IFormFile file);
    Task<string> TestPost<TRequest>(TRequest content);
    //Task<String> TestDelete();
    Task<string> CallUploadEndpoint(IFormFile file);
    //Task<(string FileContentBase64, string ContentType, string FileName)> CallDownloadEndpoint();
    //(string fileName, string version) RemoveLastVersion();
    //List<(string filename, string version)> RemoveAllVersionsAsync();
    //Task<string> Upload(IFormFile file);
    //(string fileContentBase64, string contentType, string fileName) DownloadLatestFile();
}
