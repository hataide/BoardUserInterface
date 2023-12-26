using BoardUserInterface.Common.Exceptions;
using BoardUserInterface.FileService.Service;
using BoardUserInterface.Helpers;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace BoardUserInterface.Service.Http;

public class GenericHttpClient : IGenericHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IFileService> _logger;

    public GenericHttpClient(IHttpClientFactory httpClientFactory, ILogger<IFileService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<TResponse> GetAsync<TResponse>(string requestUri, Dictionary<string, string> headers = null)
    {
        var client = _httpClientFactory.CreateClient();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await client.GetAsync(requestUri);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            // Log error details here
            throw new HttpRequestFailedException(response.StatusCode, content);
            //throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
        }

        // var content = await response.Content.ReadAsStringAsync();
        if (!JsonHelper.IsValidJson(content))
        {
            throw new InvalidOperationException("The response content is not valid JSON.");
        }

        // Deserialize the response content into an instance of TResponse
        return JsonConverterHelper.Deserialize<TResponse>(content);
    }

    public async Task<TResponse> DeleteAsync<TResponse>(string requestUri, Dictionary<string, string> headers = null)
    {
        var client = _httpClientFactory.CreateClient();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await client.DeleteAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestFailedException(response.StatusCode, errorContent);
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonConverterHelper.Deserialize<TResponse>(content);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest content, Dictionary<string, string> headers = null)
    {
        var client = _httpClientFactory.CreateClient();
        var jsonContent = new StringContent(JsonConverterHelper.SerializeObject(content), System.Text.Encoding.UTF8, "application/json");

        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await client.PostAsync(requestUri, jsonContent);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestFailedException(response.StatusCode, errorContent);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        //return JsonConverterHelper.Deserialize<TResponse>(responseContent);
        // If the expected response type is string, cast the response content and return
        if (typeof(TResponse) == typeof(string))
        {
            return (TResponse)(object)responseContent;
        }

        // Otherwise, deserialize the JSON content to the expected type
        try
        {
            return JsonConverterHelper.Deserialize<TResponse>(responseContent);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Error deserializing the response content.");
            throw new NotAbleToDeserializeException("Error deserializing the response content.", jsonEx);
        }

    }


    public async Task<TResponse> PutAsync<TRequest, TResponse>(string requestUri, TRequest content, Dictionary<string, string> headers = null)
    {
        var client = _httpClientFactory.CreateClient();
        var jsonContent = new StringContent(JsonConverterHelper.SerializeObject(content), System.Text.Encoding.UTF8, "application/json");

        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await client.PutAsync(requestUri, jsonContent);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestFailedException(response.StatusCode, errorContent);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        // If the expected response type is string, return the response content directly.
        if (typeof(TResponse) == typeof(string))
        {
            return (TResponse)(object)responseContent;
        }

        // Otherwise, deserialize the JSON content to the expected type.
        return JsonConverterHelper.Deserialize<TResponse>(responseContent);

    }


}
