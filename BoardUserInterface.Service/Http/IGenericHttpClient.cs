using BoardUserInterface.Service.Http.Response;
using System.Threading.Tasks;

namespace BoardUserInterface.Service.Http;
public interface IGenericHttpClient
{
    Task<DownloadResponse> GetAsync(string requestUri, Dictionary<string, string> headers = null);
    Task<string> PostAsync<TRequest>(string requestUri, TRequest content, Dictionary<string, string> headers = null);
    Task<string> PutAsync<TRequest>(string requestUri, TRequest content, Dictionary<string, string> headers = null);
    Task<HttpResponseMessage> DeleteAsync(string requestUri, Dictionary<string, string> headers = null);

}

