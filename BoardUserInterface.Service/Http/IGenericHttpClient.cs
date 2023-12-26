namespace BoardUserInterface.Service.Http;
public interface IGenericHttpClient
{
    Task<TResponse> GetAsync<TResponse>(string requestUri, Dictionary<string, string> headers = null);

    Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest content, Dictionary<string, string> headers = null);

    //Task<string> PutAsync<TRequest>(string requestUri, TRequest content, Dictionary<string, string> headers = null);
    Task<TResponse> PutAsync<TRequest, TResponse>(string requestUri, TRequest content, Dictionary<string, string> headers = null);
    Task<TResponse> DeleteAsync<TResponse>(string requestUri, Dictionary<string, string> headers = null);
}

