using BoardUserInterface.Service.Http.Response;

namespace BoardUserInterface.Service.Http;

public interface IGenericService
{
    Task<DownloadResponse> TestGet();

    Task<String> TestPost(String sContent);
    Task<string> PutTest(String data);
    Task<String> TestDelete();
}
